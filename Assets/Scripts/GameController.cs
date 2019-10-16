using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Vector3 rocketStartingPosition;

    public CameraController mainCameraController;
    public InputController inputController;
    public GameObject rocketPrefab;
    public GameObject blackHolePrefab;
    public StatsController statsController;

    public GameObject rocketGObject;
    public GameObject blackHoleGObject;

    public Canvas screenOverlayCanvas;

    private RocketController rocketController;
    private BlackHoleController blackHoleController;

    public bool inPlay;
    public bool cameraFollow;
    // when true, shot statistics are displayed under the finger during gesture
    public bool displayStatistics;

    private float canvasWidth, canvasHeight;

    // Start is called before the first frame update
    void Start()
    {
        inPlay = false;
        SetCameraFollowMode(false);

        canvasWidth = screenOverlayCanvas.GetComponent<RectTransform>().rect.width;
        canvasHeight = screenOverlayCanvas.GetComponent<RectTransform>().rect.height;

        // sets a callback on inputController, works like magic.
        inputController.whenUpdated += GestureUpdated;
        inputController.whenEnded += GestureEnded;

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

        Vector3 force = blackHoleController.GetGravitationalForce(rocketGObject.transform.position);
        rocketController.ApplyGravitationalForce(force);
    }

    public void ToggleCameraFollowMode()
    {
        SetCameraFollowMode(!cameraFollow);
    }

    internal void SetCameraFollowMode(bool mode)
    {
        mainCameraController.mode = (cameraFollow = mode) ? CameraMode.FollowRocket : CameraMode.Neutral;
    }

    internal void ObjectHitBlackHole(GameObject thing)
    {
        if (thing.tag == "Player")
        {
            GameOver();
        }

        Destroy(thing);
    }

    private void GameOver()
    {
        inPlay = false;
        Debug.Log("Game Over!");
    }

    private void ShootRocket(float angle, int power) 
    {
        inPlay = true; // starts gravity
        displayStatistics = false;
        rocketController.LaunchRocket(angle, power);
        SetCameraFollowMode(true);
    }

    private void AimRocketAtAngle(float angle)
    {
        rocketController.AimAtAngle(angle);
    }

    private void ShotCancelled()
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

    internal Vector3 CurrentRocketPosition()
    {
        return rocketGObject.transform.position;
    }

    internal void GestureEnded(InputConstants input)
    {
        if (inPlay)
        {
            mainCameraController.offsetFromCenter = Vector3.zero;
            return;
        }

        if (input.gesturePower > 0)
        {
            ShootRocket(input.gestureZAngleOffset, input.gesturePower);
        }
        else
        {
            ShotCancelled();
        }
        statsController.ResetFields();
    }

    internal void GestureUpdated(InputConstants input)
    {
        if (inPlay)
        {
            mainCameraController.offsetFromCenter = input.cameraOffset;
        }
        else
        {
            AimRocketAtAngle(input.gestureZAngleOffset);
            statsController.DisplayShotStatistics(input, canvasWidth, canvasHeight);
        }
    }
}
