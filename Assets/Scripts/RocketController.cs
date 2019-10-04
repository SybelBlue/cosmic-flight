using System.Collections;
using System.Collections.Generic;
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
        rigidbody.velocity = new Vector2(10, -10);
        //////////////////////////////
    }

    // Update is called once per frame
    void Update()
    {
        var spriteTransform = transform.GetChild(0).transform;
        var spriteRot = spriteTransform.rotation;

        var zRotDegrees = Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x) * 180 / Mathf.PI;

        spriteRot.eulerAngles = new Vector3(0, 0, zRotDegrees + zAngleOffset);
        spriteTransform.rotation = spriteRot;

        ///// FOR TESTING ONLY /////
        Debug.DrawLine(transform.position, ((Vector2) transform.position) + rigidbody.velocity * 10, Color.green);
        ///////////////////////////
    }
}
