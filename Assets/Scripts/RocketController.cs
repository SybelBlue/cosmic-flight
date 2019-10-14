using UnityEngine;

public class RocketController : MonoBehaviour
{

    public GameObject sprite;
    // for when a sprite (like the one I started with randomly) is crooked.
    public float zAngleOffset;

    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = transform.GetComponent<Rigidbody2D>();

        ////// FOR TESTING ONLY ///////
        // Gives initial speed
        rigidbody.velocity = new Vector2(10, -10);
        //////////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        var spriteTransform = transform.GetChild(0).transform;
        var spriteRot = spriteTransform.rotation;
        
        var zRotDegrees = Mathf.Rad2Deg * Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x);

        spriteRot.eulerAngles = new Vector3(0, 0, zRotDegrees + zAngleOffset);
        spriteTransform.rotation = spriteRot;

        ///// FOR TESTING ONLY /////
        // Adds line from rocket representing velocity
        Debug.DrawLine(transform.position, ((Vector2) transform.position) + rigidbody.velocity * 3, Color.green);
        ///////////////////////////
    }

    public void ApplyGravitationalForce(Vector2 force)
    {
        rigidbody.AddForce(force);
    }

    public void AddRelativiticStretch(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void AimAtAngle(float angle) 
    {
        var rigidbodyTransform = transform;
        var rigidbodyRot = rigidbodyTransform.rotation;

        var rigidbodyDeg = Mathf.Rad2Deg * Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x);

        rigidbodyRot.eulerAngles = new Vector3(0, 0, rigidbodyDeg + angle);
        rigidbodyTransform.rotation = rigidbodyRot;

    }
}
