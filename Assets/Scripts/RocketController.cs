using UnityEngine;

public class RocketController : MonoBehaviour
{
    public GameObject planetToLandOn, planetLandedOn;
    public GameObject sprite;
    public float defaultAngle;

    public Rigidbody2D rigidbody;

    public float velocityPerPower;
    public bool showActualSize;
    
    internal bool isSafe { get { return planetToLandOn != null || planetLandedOn != null;  } }

    private void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();
        showActualSize = false;
        ResetRotation();
    }

    internal void ResetRotation()
    {
        AimAtAngle(0);
    }

    // Update is called once per frame
    void Update()
    {
        Rescale();


        if (planetToLandOn != null)
        {
            if ((planetToLandOn.transform.position - transform.position).sqrMagnitude > 1)
            {
                transform.position = Vector3.Slerp(transform.position, planetToLandOn.transform.position, 0.2f);
            }
            else
            {
                transform.position = planetToLandOn.transform.position;
                planetLandedOn = planetToLandOn;
                planetToLandOn = null;
            }
        }

        ///// FOR TESTING ONLY /////
        // Adds line from rocket representing velocity
        Debug.DrawLine(transform.position, ((Vector2)transform.position) + rigidbody.velocity * 3, Color.green);
        ///////////////////////////
    }

    /// <summary>
    /// Performs one frame of smoothed scaling of the rocket to the
    /// size determined by showActualSize
    /// </summary>
    private void Rescale()
    {
        float targetScale = !showActualSize  && false ? 1f : 0.5f;
        float newScale = Mathf.Lerp(transform.localScale.x, targetScale, 0.2f);
        transform.localScale = new Vector3(newScale, newScale);
    }

    /// <summary>
    /// Applies force to this object and then rotates so that it
    /// is facing in the direction of its velocity
    /// </summary>
    /// <param name="force">the force to apply to this</param>
    internal void ApplyGravitationalForce(Vector2 force)
    {
        rigidbody.AddForce(force);

        var newRotation = transform.rotation;

        var zRotDegrees = Mathf.Rad2Deg * Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x);

        newRotation.eulerAngles = new Vector3(0, 0, zRotDegrees);
        transform.rotation = newRotation;
    }

    /// <summary>
    /// Rescales this to scale.
    /// TODO: use this in conjuction with distance from black hole,
    /// maybe take position of black hole instead of scale?
    /// </summary>
    /// <param name="scale">the new scale of this</param>
    internal void AddRelativiticStretch(Vector3 scale)
    {
        transform.localScale = scale;
    }

    /// <summary>
    /// Gets the angle relative to the start position and aims there
    /// </summary>
    /// <param name="angle">angle in standard degrees, with 0 at +x</param>
    internal void AimAtAngle(float angle)
    {
        AimAtAbsoluteAngle(GetRelativeAngle(angle));
    }

    /// <summary>
    /// Rotates to face that angle, regardless of sprite standard starting angle
    /// </summary>
    /// <param name="angle">angle in standard degrees, with 0 at satndard starting angle</param>
    private void AimAtAbsoluteAngle(float angle)
    {
        var rigidbodyTransform = transform;
        var rigidbodyRot = rigidbodyTransform.rotation;

        rigidbodyRot.eulerAngles = new Vector3(0, 0, angle); // TODO: change back to angle?
        rigidbodyTransform.rotation = rigidbodyRot;
    }

    /// <summary>
    /// Performs all computation to launch the rocket, sets the rocket to return
    /// to its regular size and unchilds from any previous planet/start-point
    /// </summary>
    /// <param name="angle">angle in standard degrees, with 0 at +x</param>
    /// <param name="power">power coefficient (positive proportional)</param>
    internal void Launch(float angle, int power)
    {
        AimAtAngle(angle);
        float adjustedAngle = Mathf.Deg2Rad * GetRelativeAngle(angle);
        // !!!!!!!!!!!!!!!!!!!!!!! MATHF TAKES RADIANS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        var newVelocity = power * velocityPerPower * new Vector2(Mathf.Cos(adjustedAngle), Mathf.Sin(adjustedAngle));
        rigidbody.velocity += newVelocity;
        showActualSize = true;
        // !!!!!!!!!!!!!!!!!!!!!!!! UNCHILD SO THAT IT FLIES STRAIGHT !!!!!!!!!!!!!!!!!!
        transform.parent = null;
        planetToLandOn = null;
        planetLandedOn = null;
    }

    /// <summary>
    /// Converts standard angle to angle relative to the sprite's starting angle
    /// </summary>
    /// <param name="standardAngle">angle in standard degrees, with 0 at +x</param>
    /// <returns></returns>
    private float GetRelativeAngle(float standardAngle)
    {
        return standardAngle + defaultAngle;
    }

    /// <summary>
    /// Sets up this object to perform a smooth landing on planet
    /// </summary>
    /// <param name="planet">the GameObject to land on</param>
    internal void LandOn(GameObject planet)
    {
        if (planetToLandOn != null) return;

        // loose all speed, end flight
        if (rigidbody != null) rigidbody.velocity = Vector3.zero;
        showActualSize = false;
        
        // aim point at planet
        Vector3 towardsPlanet = planet.transform.position - this.transform.position;
        float angle = Mathf.Atan2(towardsPlanet.y, towardsPlanet.x);
        AimAtAbsoluteAngle(Mathf.Rad2Deg * angle);

        // make a child of the planet so it will rotate with it
        transform.parent = planet.transform;
        planetToLandOn = planet;
    }
}
