using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Vector3 rocketStartingPosition;

    public GameObject rocketPrefab;
    public GameObject blackHolePrefab;

    public GameObject rocketGObject;
    public GameObject blackHoleGObject;

    public Canvas screenOverlayCanvas;

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

        rocketController.ApplyGravitationalForce(
            blackHoleController.GetGravitationalForce(rocketGObject.transform.position)
            );
    }

    public void ObjectHitBlackHole(GameObject thing)
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

    internal void ShootRocket(float angle, int power) 
    {

        Debug.Log("Fire! pow:" + power + ", angle:" + angle);
        inPlay = true; // starts gravity
        // more shooting logic here
    }

    internal void AimRocketAtAngle(float angle)
    {
        rocketController.AimAtAngle(angle);
    }

    internal void ShotCancelled()
    {
        rocketController.ResetRotation();
    }

    internal float GetOverlayCanvasWidth()
    {
        return screenOverlayCanvas.GetComponent<RectTransform>().rect.width;
    }

    internal float GetOverlayCanvasHeight()
    {
        return screenOverlayCanvas.GetComponent<RectTransform>().rect.height;
    }
}
