using System;
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

    public bool inPlay;

    // Start is called before the first frame update
    void Start()
    {
        inPlay = false;

        blackHoleGObject = Instantiate(blackHolePrefab);
        blackHoleController = blackHoleGObject.GetComponent<BlackHoleController>();
        blackHoleController.gameController = this;

        rocketGObject = Instantiate(rocketPrefab, rocketStartingPosition, Quaternion.Euler(0, 0, 0));
        rocketController = rocketGObject.GetComponent<RocketController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing if not in play
        if (!inPlay) return;

        var force = blackHoleController.GetGravitationalForce(rocketGObject);
        rocketController.ApplyGravitationalForce(force);
    }

    public void ObjectDied(GameObject thing)
    {
        if (thing.tag == "Player")
        {
            GameOver();
        }
        Destroy(thing);
    }

    void GameOver()
    {
        inPlay = false;
        Debug.Log("Game Over!");
    }

    public Vector3 GetRocketPosition()
    {
        return rocketGObject.transform.position;
    }

    public void ShootRocket(float angle, int power) //add angle and power params?
    {

        Debug.Log("Fire! pow:" + power + ", angle:" + angle);
        inPlay = true; // starts gravity
        // more shooting logic here
    }

    internal void AimRocketAtAngle(float angle)
    {
        rocketController.AimAtAngle(angle);
    }

    internal float GetOverlayCanvasWidth()
    {
        throw new NotImplementedException();
    }
}
