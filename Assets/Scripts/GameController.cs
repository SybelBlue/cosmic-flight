﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Vector3 rocketStartingPosition;
    public Vector3 planetPosition;
    public Vector3[] asteroidStartingPositions;

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

    public GameObject rocketGObject;
    public GameObject blackHoleGObject;
    public List<GameObject> planetGObjects;
    public List<GameObject> asteroidGObjects;

    public Canvas screenOverlayCanvas;

    public RetryButtonController retryButton;


    // 1 is normal speed, 0.5 is half speed. I love Unity.
    public float timeScale;

    private RocketController rocketController;
    private BlackHoleController blackHoleController;

    private List<GameObject> claimedAsteroids;

    private GameObject lastSafeLanding;

    public bool inPlay;
    // when true, shot statistics are displayed under the finger during gesture
    public bool displayStatistics;

    private float canvasWidth, canvasHeight;

    // Start is called before the first frame update
    void Start()
    {
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

        retryButton.gameObject.SetActive(false);
    }

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

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Play()
    {
        Time.timeScale = timeScale;
    }

    void FixedUpdate()
    {
        relaunchButton.SetActive(inPlay);

        // Do nothing if not in play
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
            if (thing.GetComponent<RocketController>().isSafe) return;

            FlightFailed();
            return;
        }

        Destroy(thing);
    }

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
            body.GetComponent<AsteroidController>().RaiseFlag();
            claimedAsteroids.Add(body);
            SetOxygenMode(OxygenMode.Landed);
            oxygenMeterController.ClaimAsteroid();
        }
    }

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

    private void GameWon()
    {
        Debug.LogWarning("YOU WON!");
        retryButton.gameObject.SetActive(true);
        SetAllowInputs(false);
    }

    public void FlightFailed()
    {
        inPlay = false;
        rocketController.LandOn(lastSafeLanding);
        SetOxygenMode(OxygenMode.Safe);
        claimedAsteroids.ForEach(asteroid => asteroid.GetComponent<AsteroidController>().LowerFlag());
        claimedAsteroids.Clear();
        inputController.displayRings = true;
    }

    private void ShootRocket(float angle, int power) 
    {
        inPlay = true; // starts gravity
        displayStatistics = false;
        inputController.displayRings = false;
        launchCounter++;

        rocketController.LaunchRocket(angle, power);
        SetCameraFollowMode(CameraMode.FollowRocket);
        SetOxygenMode(OxygenMode.Flying);
    }

    private void SetOxygenMode(OxygenMode mode)
    {
        oxygenMeterController.mode = mode;
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

    internal void OutOfOxygen()
    {
        Debug.Log("Out of Oxygen!");
        FlightFailed();
    }

    internal void SetAllowInputs(bool value)
    {
        inputController.acceptingInputs = value;
    }
}
