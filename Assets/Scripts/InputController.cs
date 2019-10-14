using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    public GameController game;

    public TouchPhase currentPhase;

    public int unitsYPerPower;
    public float degreesPerUnitX;

    public int gesturePower;
    public float gestureZAngleOffset;

    private Vector3 gestureStartPosition;
    private Vector3 gestureDelta;

    private void Start()
    {
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
            OnControlGestureStart(touch.position);
        }
        else if (touch.phase == TouchPhase.Canceled)
        {
            ResetFields();
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            OnControlRelease();
        }
        else
        {
            OnControlDrag(touch.position);
        }
    }

    private void CheckForClicks()
    {
        // frame that it was pressed down?
        if (Input.GetMouseButtonDown(0))
        {
            OnControlGestureStart(Input.mousePosition);
        }
        // frame that it was released?
        else if (Input.GetMouseButtonUp(0))
        {
            OnControlRelease();
        }
        // is currently held down?
        else if (Input.GetMouseButton(0))
        {
            OnControlDrag(Input.mousePosition);
        }
    }

    void OnControlGestureStart(Vector3 startPosition)
    {
        // display rings around rocket, display touch point
        currentPhase = TouchPhase.Began;
        gestureStartPosition = startPosition;

        UpdateGestureProperties();
    }

    void OnControlDrag(Vector3 currentPosition)
    {
        // change angle, change level
        currentPhase = TouchPhase.Moved;
        gestureDelta = gestureStartPosition - currentPosition;

        UpdateGestureProperties();
    }

    void OnControlRelease()
    {
        if (gesturePower > 0)
        {
            game.Shoot(gestureZAngleOffset, gesturePower);
        }
        else
        {
            Debug.Log("Fire cancelled.");
        }

        ResetFields();
    }

    private void ResetFields()
    {
        currentPhase = TouchPhase.Ended;
        gestureStartPosition = Vector3.zero;
        gestureDelta = Vector3.zero;
        gesturePower = 0;
        gestureZAngleOffset = 0;
    }

    private void UpdateGestureProperties()
    {
        gesturePower = (int)Mathf.Clamp(gestureDelta.y / unitsYPerPower + 2, 0, 3);
        gestureZAngleOffset = Mathf.Clamp(degreesPerUnitX * gestureDelta.x, -180, 180);

        game.AimRocketAtAngle(gestureZAngleOffset);
    }
}
