using System;
using UnityEngine;

public class RocketController : MonoBehaviour
{

    public GameObject sprite;
    public float defaultAngle;

    private Rigidbody2D rigidbody;

    public float velocityPerPower;

    private void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();

        ResetRotation();

        ////// FOR TESTING ONLY ///////
        // Gives initial speed
        // rigidbody.velocity = new Vector2(10, -10);
        //////////////////////////////
    
    }

    internal void ResetRotation()
    {
        AimAtAngle(0);
    }

    // Update is called once per frame
    void Update()
    {
        ///// FOR TESTING ONLY /////
        // Adds line from rocket representing velocity
        Debug.DrawLine(transform.position, ((Vector2) transform.position) + rigidbody.velocity * 3, Color.green);
        ///////////////////////////
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
        var rigidbodyTransform = transform;
        var rigidbodyRot = rigidbodyTransform.rotation;

        rigidbodyRot.eulerAngles = new Vector3(0, 0, GetRelativeAngle(angle));
        rigidbodyTransform.rotation = rigidbodyRot;
    }

    public void LaunchRocket(float angle, int power)
    {
        AimAtAngle(angle);

        float adjustedAngle = Mathf.Deg2Rad * GetRelativeAngle(angle);
        /// !!!!!!!!!!!!!!!!!!!!!!! MATHF TAKES RADIANS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        var newVelocity = power * velocityPerPower * new Vector2(Mathf.Cos(adjustedAngle), Mathf.Sin(adjustedAngle));
        rigidbody.velocity += newVelocity;
    }

    private float GetRelativeAngle(float standardAngle)
    {
        return standardAngle + defaultAngle;
    }
}
