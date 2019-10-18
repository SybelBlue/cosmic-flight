using System;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    public GameObject planetToLandOn, planetLandedOn;
    public GameObject sprite;
    public float defaultAngle;

    public Rigidbody2D rigidbody;

    public float velocityPerPower;
    public bool showActualSize;

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

    private void Rescale()
    {
        float targetScale = !showActualSize ? 1f : 0.5f;
        float newScale = Mathf.Lerp(transform.localScale.x, targetScale, 0.2f);
        transform.localScale = new Vector3(newScale, newScale);
    }

    internal void ApplyGravitationalForce(Vector2 force)
    {
        rigidbody.AddForce(force);

        var newRotation = transform.rotation;

        var zRotDegrees = Mathf.Rad2Deg * Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x);

        newRotation.eulerAngles = new Vector3(0, 0, zRotDegrees);
        transform.rotation = newRotation;

    }

    internal void AddRelativiticStretch(Vector3 scale)
    {
        transform.localScale = scale;
    }

    internal void AimAtAngle(float angle)
    {
        AimAtAbsoluteAngle(GetRelativeAngle(angle));
    }

    private void AimAtAbsoluteAngle(float angle)
    {
        var rigidbodyTransform = transform;
        var rigidbodyRot = rigidbodyTransform.rotation;

        rigidbodyRot.eulerAngles = new Vector3(0, 0, angle);
        rigidbodyTransform.rotation = rigidbodyRot;
    }

    public void LaunchRocket(float angle, int power)
    {
        AimAtAngle(angle);
        float adjustedAngle = Mathf.Deg2Rad * GetRelativeAngle(angle);
        /// !!!!!!!!!!!!!!!!!!!!!!! MATHF TAKES RADIANS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        var newVelocity = power * velocityPerPower * new Vector2(Mathf.Cos(adjustedAngle), Mathf.Sin(adjustedAngle));
        rigidbody.velocity += newVelocity;
        showActualSize = true;
        planetLandedOn = null;
    }

    private float GetRelativeAngle(float standardAngle)
    {
        return standardAngle + defaultAngle;
    }

    internal void LandOn(GameObject planet)
    {
        // loose all speed, end flight
        rigidbody.velocity = Vector3.zero;
        showActualSize = false;
        
        // aim base at planet
        Vector3 awayFromPlanet = this.transform.position - planet.transform.position;
        float angle = Mathf.Atan2(awayFromPlanet.y, awayFromPlanet.x);
        AimAtAbsoluteAngle(Mathf.Rad2Deg * angle);

        // make a child of the planet so it will rotate with it
        transform.parent = planet.transform;
        planetToLandOn = planet;
    }
}
