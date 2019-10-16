using System;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    // game controller to report events to
    public GameController game;

    // statistics text game object...
    public GameObject statsGObject;
    // ...and text
    private Text statsText;

    // current simplified gesture phase, (Began, Moved, Ended only)
    public TouchPhase currentPhase;

    // pixels dragged in the y to increase power one step
    public int unitsYPerPower;
    // degrees to increase for each pixel dragged in the x
    public float degreesPerUnitX;

    // computed current gesture power (0 when currentPhase is Ended)
    public int gesturePower;
    // computed current gesture angle offset (0 when currentPhase is Ended)
    public float gestureZAngleOffset;

    // current gesture first touch (Vector3.zero when currentPhase is Ended)
    public Vector3 gestureStartPosition;
    // computed current gesture delta from first touch (Vector3.zero when currentPhase is Ended)
    public Vector3 gestureDelta;

    // scalar factor * 100 to move camera when peeking around during flight
    public float peekSensitivity;
    // when true, shot statistics are displayed under the finger during gesture
    public bool displayStatistics;

    private void Start()
    {
        displayStatistics = true;
        ResetFields();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTouches();

        CheckForClicks();
    }

    private void CheckForTouches()
    {
        // taken from https://docs.unity3d.com/Manual/MobileInput.html   
        if (Input.touches.Length == 0) return; // if no touches, then exit

        Touch touch = Input.touches[0];
        if (currentPhase == TouchPhase.Ended)
        {
            OnGestureStart(touch.position);
        }
        else if (touch.phase == TouchPhase.Canceled)
        {
            ResetFields();
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            OnGestureRelease();
        }
        else
        {
            OnGestureDrag(touch.position);
        }
    }

    private void CheckForClicks()
    {
        // frame that it was pressed down?
        if (Input.GetMouseButtonDown(0))
        {
            OnGestureStart(Input.mousePosition);
        }
        // frame that it was released?
        else if (Input.GetMouseButtonUp(0))
        {
            OnGestureRelease();
        }
        // is currently held down?
        else if (Input.GetMouseButton(0))
        {
            OnGestureDrag(Input.mousePosition);
        }
    }

    void OnGestureStart(Vector3 startPosition)
    {
        // display rings around rocket, display touch point
        currentPhase = TouchPhase.Began;
        gestureStartPosition = startPosition;

        UpdateGestureProperties(startPosition);
    }

    void OnGestureDrag(Vector3 currentPosition)
    {
        currentPhase = TouchPhase.Moved;
        gestureDelta = gestureStartPosition - currentPosition;

        UpdateGestureProperties(currentPosition);
    }

    void OnGestureRelease()
    {
        game.GestureEnded(this);

        ResetFields();
    }

    private void ResetFields()
    {
        currentPhase = TouchPhase.Ended;
        gestureStartPosition = Vector3.zero;
        gestureDelta = Vector3.zero;
        gesturePower = 0;
        gestureZAngleOffset = 0;

        statsGObject.SetActive(false);
        if (statsText == null)
        {
            statsText = statsGObject.GetComponent<Text>();
        }
        statsText.text = "Stats Text\nStats Text 2";
    }

    private void UpdateGestureProperties(Vector3 position)
    {
        gesturePower = (int)Mathf.Clamp(gestureDelta.y / unitsYPerPower + 2, 0, 3);
        gestureZAngleOffset = Mathf.Clamp(degreesPerUnitX * gestureDelta.x, -180, 180);

        game.GestureUpdated(this);

        if (displayStatistics)
        {
            DisplayShotStatistics(position);
        }
    }

    private void DisplayShotStatistics(Vector3 position)
    {
        statsGObject.SetActive(true);
        // TODO: maybe later add invisible game object at touch position, child statsGObject to it here
        statsGObject.transform.position = position + new Vector3(0, 20, 0);

        var newPos = statsGObject.transform.position;

        // weird units here, hence the *2 and the *4, though I'm not sure why...

        // clamp the object so that it can't be offscreen in the x direction...
        float halfWidth = statsGObject.GetComponent<RectTransform>().rect.width / 2;
        newPos.x = Mathf.Clamp(newPos.x, halfWidth, game.GetOverlayCanvasWidth() - halfWidth * 2);

        // ... or the y direction
        float halfHeight = statsGObject.GetComponent<RectTransform>().rect.height / 2;
        newPos.y = Mathf.Clamp(newPos.y, halfHeight, game.GetOverlayCanvasHeight() - 4 * halfHeight);

        statsGObject.transform.position = newPos;

        // set the text
        statsText.text = (gesturePower > 0) ?
            string.Format("Power Level: {0}\nAngle: {1}",
                gesturePower,
                (int)gestureZAngleOffset
            ) :
            "Cancel\nShot";
    }
}
