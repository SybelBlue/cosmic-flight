using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Vector3 rocketStartingPosition;

    public GameObject rocketPrefab;
    public GameObject blackHolePrefab;

    public GameObject rocketGObject;
    public GameObject blackHoleGObject;

    private RocketController rocketController;
    private BlackHoleController blackHoleController;

    private Rigidbody2D rocketRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        blackHoleGObject = Instantiate(blackHolePrefab);
        blackHoleController = blackHoleGObject.GetComponent<BlackHoleController>();

        rocketGObject = Instantiate(rocketPrefab, rocketStartingPosition, Quaternion.Euler(0, 0, 0));
        rocketController = rocketGObject.GetComponent<RocketController>();
        rocketRigidbody = rocketGObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var force = blackHoleController.GetGravitationalForce(rocketGObject);
        rocketRigidbody.AddForce(force);
    }
}
