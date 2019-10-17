using System;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Vector3 rocketStartingPosition;
    public Vector3 exitPlanetPosition;

    public CameraController mainCameraController;
    public InputController inputController;

    public GameObject rocketPrefab;
    public GameObject blackHolePrefab;
    public GameObject exitPlanetPrefab;

    public GameObject cameraAngleButton;
    public StatsController statsController;

    public GameObject rocketGObject;
    public GameObject blackHoleGObject;
    public GameObject exitPlanetGObject;

    public Canvas screenOverlayCanvas;

    private RocketController rocketController;
    private BlackHoleController blackHoleController;
    private ExitPlanetController exitPlanetController;

    public bool inPlay;
    // when true, shot statistics are displayed under the finger during gesture
    public bool displayStatistics;

    private float canvasWidth, canvasHeight;

    // Start is called before the first frame update
    void Start()
    {
        inPlay = false;
        SetCameraFollowMode(CameraMode.Neutral);

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

        exitPlanetGObject = Instantiate(exitPlanetPrefab, exitPlanetPosition, Quaternion.Euler(0, 0, 0));
        exitPlanetController = exitPlanetGObject.GetComponent<ExitPlanetController>();
        exitPlanetController.gameController = this;
    }

    void Update()
    {
        // Do nothing if not in play
        cameraAngleButton.SetActive(inPlay);
        if (!inPlay) return;

        Vector3 force = blackHoleController.GetGravitationalForce(rocketGObject.transform.position);
        rocketController.ApplyGravitationalForce(force);
    }

    public void ToggleCameraFollowMode()
    {
        SetCameraFollowMode(
            mainCameraController.mode == CameraMode.Neutral ?
                CameraMode.FollowRocket :
                CameraMode.Neutral
            ) ;
    }

    internal void SetCameraFollowMode(CameraMode mode)
    {
        mainCameraController.mode = mode;
    }

    internal void ObjectHitBlackHole(GameObject thing)
    {
        if (thing.tag == "Player")
        {
            GameOver();
            rocketController = null;
        }

        Destroy(thing);
    }

    internal void LandOnPlanet(GameObject planet)
    {
        inPlay = false;
        SetCameraFollowMode(CameraMode.Neutral);
        rocketController.LandOn(planet);
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
        SetCameraFollowMode(CameraMode.FollowRocket);
    }

    private void AimRocketAtAngle(float angle)
    {
        rocketController.AimAtAngle(angle);
    }

    private void ShotCancelled()
    {
        rocketController.ResetRotation();
        SetCameraFollowMode(CameraMode.Neutral);
    }

    internal Vector3 CurrentRocketPosition()
    {
        return rocketGObject != null? 
            rocketGObject.transform.position :
        // presumably the rocket was sucked into the black hole and destroyed
            blackHoleGObject.transform.position;
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
            SetCameraFollowMode(CameraMode.FollowRocket);
            AimRocketAtAngle(input.gestureZAngleOffset);
            statsController.DisplayShotStatistics(input, canvasWidth, canvasHeight);
        }
    }
}
