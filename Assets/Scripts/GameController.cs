﻿using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // all for defaults and testing: level data replaces this //
    public Vector3 rocketStartingPosition;                    //
    public Vector3 planetPosition;                            //
    public Vector3[] asteroidStartingPositions;               //
    // /////////////////////////////////////////////////////////

    public CameraController mainCameraController;
    public InputController inputController;

    public GameObject rocketPrefab;
    public GameObject blackHolePrefab;
    public GameObject planetPrefab;
    public GameObject asteroidPrefab;

    public GameObject cameraAngleButton;
    public StatsController statsController;
    public OxygenMeterController oxygenMeterController;
    public GameObject relaunchButton;
    public CounterController launchCounter, asteroidCounter, planetCounter;

    public GameObject endOfLevelButtons;

    // supposed to be 0-length or null until filled by this //
    public GameObject rocketGObject;                        //
    public GameObject blackHoleGObject;                     //
    public List<GameObject> planetGObjects;                 //
    public List<GameObject> asteroidGObjects;               //
    // ///////////////////////////////////////////////////////

    public Canvas screenOverlayCanvas;

    // 1 is normal speed, 0.5 is half speed. I love Unity.
    public float timeScale;

    private GameObject levelDataObject;

    private RocketController rocketController;
    private BlackHoleController blackHoleController;

    private List<GameObject> claimedAsteroids;

    private GameObject lastSafeLanding;

    /// <summary>
    /// True when rocket is in flight
    /// </summary>
    public bool inPlay;
    /// <summary>
    /// When true, shot statistics are displayed above the finger during gesture input
    /// </summary>
    public bool displayStatistics;

    private float canvasWidth, canvasHeight;

    // Start is called before the first frame update
    void Start()
    {
        LoadLevelData();

        // God I love Unity
        Time.timeScale = timeScale;
        inPlay = false;
        inputController.displayRings = true;
        lastSafeLanding = new GameObject("Starting Position");
        lastSafeLanding.transform.position = rocketStartingPosition;

        claimedAsteroids = new List<GameObject>();

        SetCameraFollowMode(CameraMode.Neutral);
        SetOxygenMode(OxygenMode.Safe);

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

        MakeNewPlanet(planetPosition);

        foreach (Vector3 position in asteroidStartingPositions)
        {
            MakeNewAsteroid(position);
        }

        endOfLevelButtons.SetActive(false);
    }

    /// <summary>
    /// Loads level data by finding the GameObject with
    /// the tag "Level Data", otherwise logs a warning and
    /// proceeds with inspector set defaults
    /// </summary>
    private void LoadLevelData()
    {
        levelDataObject = GameObject.FindGameObjectWithTag("Level Data");
        if (levelDataObject == null)
        {
            Debug.LogWarning("Level Data Object Not Found!");
            return;
        }

        LevelManager levelManager = levelDataObject.GetComponent<LevelManager>();

        this.rocketStartingPosition = levelManager.rocketStartingPosition;
        this.planetPosition = levelManager.planetPosition;
        this.asteroidStartingPositions = levelManager.asteroidStartingPostions;
    }

    /// <summary>
    /// Makes and returns a new planet GameObject at position,
    /// updating the planetCounter and planetGObjects fields 
    /// </summary>
    /// <param name="position">position in world space for the new instance</param>
    /// <returns>new instance of a planet</returns>
    private GameObject MakeNewPlanet(Vector3 position)
    {
        if (planetGObjects == null)
        {
            planetGObjects = new List<GameObject>();
        }

        var planet = Instantiate(planetPrefab, position, Quaternion.Euler(0, 0, 0));
        planet.GetComponent<PlanetController>().gameController = this;
        planetGObjects.Add(planet);
        planetCounter++;

        return planet;
    }

    /// <summary>
    /// Makes and returns a new asteroid GameObject at position,
    /// updating the asteroidCounter and asteroidGObjects fields 
    /// </summary>
    /// <param name="position">position in world space for the new instance</param>
    /// <returns>new instance of a asteroid</returns>
    private GameObject MakeNewAsteroid(Vector3 position)
    {
        if (asteroidGObjects == null)
        {
            asteroidGObjects = new List<GameObject>();
        }

        var asteroid = Instantiate(asteroidPrefab, position, Quaternion.Euler(0, 0, 0));
        asteroid.GetComponent<AsteroidController>().gameController = this;
        asteroidGObjects.Add(asteroid);
        asteroidCounter++;

        return asteroid;
    }

    /// <summary>
    /// Convenience method for a pause button
    /// </summary>
    public void Pause()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Designed to resume play at same speed as before
    /// </summary>
    public void Play()
    {
        Time.timeScale = timeScale;
    }

    /// <summary>
    /// Advance to the next level, with logging
    /// Called by inspector in Button.OnClick lists
    /// </summary>
    public void NextLevel()
    {
        Debug.Log("called next level");
        if (levelDataObject == null) return;
        Debug.Log("working");
        LevelManager levelManager = levelDataObject.GetComponent<LevelManager>();
        levelManager.levelNumber++;
        levelManager.levelNumber %= levelManager.levels.Length;
    }

    /// <summary>
    /// Called every consistently in time, as opposed to Update
    /// or LateUpdate which are frame by frame. Makes it so lag
    /// on per device basis and timeScale changes doesn't 
    /// change the simulated exertion of force on the rocket by
    /// the black hole
    /// </summary>
    void FixedUpdate()
    {
        relaunchButton.SetActive(inPlay);

        // Do nothing if not in play
        if (!inPlay) return;

        Vector3 force = blackHoleController.GetGravitationalForce(rocketGObject.transform.position);
        rocketController.ApplyGravitationalForce(force);
    }

    /// <summary>
    /// Called by the Camera Mode Toggle Button.OnClick 
    /// </summary>
    public void ToggleCameraFollowMode()
    {
        SetCameraFollowMode(
            mainCameraController.mode == CameraMode.Neutral ?
                CameraMode.FollowRocket :
                CameraMode.Neutral
            );
    }

    /// <summary>
    /// Called internally on rocket flight events
    /// </summary>
    /// <param name="mode">new mode for the camera</param>
    internal void SetCameraFollowMode(CameraMode mode)
    {
        mainCameraController.mode = mode;
    }

    /// <summary>
    /// Whenever something with a collider interacts with the
    /// black hole's event horizon, this is called on it.
    /// </summary>
    /// <param name="thing">thing which hit event horizon</param>
    internal void ObjectHitBlackHole(GameObject thing)
    {
        if (thing.tag == "Player")
        {
            if (thing.GetComponent<RocketController>().isSafe) return;

            FlightFailed();
            return;
        }

        Destroy(thing);
    }

    /// <summary>
    /// Sets the rocket to land on an object, filtering
    /// for accidental collision during reset on death,
    /// reseting the camera and checking if its a safe
    /// landing zone or not and adjusts the O2 meter
    /// accordingly
    /// </summary>
    /// <param name="body">body encountered by rocket</param>
    internal void RocketLandOn(GameObject body)
    {
        inPlay = false;
        SetCameraFollowMode(CameraMode.Neutral);
        rocketController.LandOn(body);
        inputController.displayRings = true;

        if (body.tag == "Planet")
        {
            lastSafeLanding = body;
            Terraform();
            SetOxygenMode(OxygenMode.Safe);
        }
        else
        {
            if (!claimedAsteroids.Contains(body))
            {
                body.GetComponent<AsteroidController>().RaiseFlag();
                claimedAsteroids.Add(body);
                oxygenMeterController.ClaimAsteroid();
            }

            SetOxygenMode(OxygenMode.Landed);
        }
    }

    /// <summary>
    /// Terraforms all asteroids with flags raised into planets,
    /// checks for a win
    /// </summary>
    private void Terraform()
    {
        foreach (GameObject asteroid in claimedAsteroids)
        {
            // make new planet has planetCounter++
            var planet = MakeNewPlanet(asteroid.transform.position); 
            planetGObjects.Add(planet);

            planet.GetComponent<PlanetController>().Replace(asteroid);
            asteroidGObjects.Remove(asteroid);
            asteroidCounter--;
        }

        claimedAsteroids.Clear();

        if (asteroidGObjects.Count == 0 && lastSafeLanding.tag == "Planet")
        {
            GameWon();
        }
    }

    /// <summary>
    /// Logs a warning, displays end of level buttons,
    /// disallows gesture inputs
    /// </summary>
    private void GameWon()
    {
        endOfLevelButtons.SetActive(true);
        SetAllowInputs(false);
    }

    /// <summary>
    /// Called when a rocket hits a black hole or runs out of O2
    /// </summary>
    public void FlightFailed()
    {
        inPlay = false;
        rocketController.LandOn(lastSafeLanding);
        SetOxygenMode(OxygenMode.Safe);
        claimedAsteroids.ForEach(asteroid => asteroid.GetComponent<AsteroidController>().LowerFlag());
        claimedAsteroids.Clear();
        inputController.displayRings = true;
    }

    /// <summary>
    /// Fires the rocket at given angle and power level
    /// </summary>
    /// <param name="angle">angle in standard degrees</param>
    /// <param name="power">power level</param>
    private void ShootRocket(float angle, int power) 
    {
        inPlay = true; // starts gravity
        displayStatistics = false;
        inputController.displayRings = false;
        launchCounter++;

        rocketController.Launch(angle, power);
        SetCameraFollowMode(CameraMode.FollowRocket);
        SetOxygenMode(OxygenMode.Flying);
    }

    /// <summary>
    /// Sets the mode of the O2 controller
    /// </summary>
    /// <param name="mode">new mode</param>
    private void SetOxygenMode(OxygenMode mode)
    {
        oxygenMeterController.mode = mode;
    }

    /// <summary>
    /// Points the rocket sprite at angle
    /// </summary>
    /// <param name="angle">angle in standard degrees</param>
    private void AimRocketAtAngle(float angle)
    {
        rocketController.AimAtAngle(angle);
    }

    /// <summary>
    /// Resets rocket rotation and camera mode
    /// </summary>
    private void ShotCancelled()
    {
        rocketController.ResetRotation();
        SetCameraFollowMode(CameraMode.Neutral);
    }

    /// <summary>
    /// Provides current rocket.transform.position if 
    /// rocket is not null
    /// </summary>
    /// <returns>rocket position</returns>
    internal Vector3 CurrentRocketPosition()
    {
        return rocketGObject != null? 
            rocketGObject.transform.position :
        // presumably the rocket was sucked into the black hole and destroyed, 
        // should be depreciated (rocket should never be null)
            blackHoleGObject.transform.position;
    }

    /// <summary>
    /// Behavior on the conclusion of a gesture. Used as delegate
    /// (callback) from InputController.whenEnded event
    /// </summary>
    /// <param name="input">gesture data</param>
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

    /// <summary>
    /// Behavior when a gesture is being inputted. Used as delegate
    /// (callback) from InputController.whenUpdated event
    /// </summary>
    /// <param name="input">gesture data</param>
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

    /// <summary>
    /// Calls GameController.FlightFailed()
    /// </summary>
    internal void OutOfOxygen()
    {
        FlightFailed();
    }

    /// <summary>
    /// Sets whether the InputControllwer will allow gesture inputs
    /// </summary>
    /// <param name="value">new bool for allowing gesture input</param>
    internal void SetAllowInputs(bool value)
    {
        inputController.acceptingInputs = value;
    }
}
