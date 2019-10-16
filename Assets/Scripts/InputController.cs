using UnityEngine;

/// <summary>
/// A form of method which will be used in GestureEvents, ie
/// {
///     ...
///     InputController controller = (some instance);
///     contoller.whenUpdated += OnGestureUpdatedMethod;
///     contoller.whenUpdated += AlsoDoThisOnUpdateMethod;
///     ...
///  }
/// .
/// .
/// .
/// private OnGestureUpdatedMethod(InputConstants constants) {
///  ...do something here with the constants which are the full state of the input controller
/// }
/// .
/// .
/// .
/// private AlsoDoThisOnUpdateMethod(InputConstants constants) {
///  ...do something here with the constants which are the full state of the input controller
/// }
/// </summary>
/// <param name="constants"></param>
public delegate void GestureEventDelegate(InputConstants constants);

/// <summary>
/// A class which only serves to store some values and pass them safely.
/// Contains the full state of an input contoller so that it may be passed safely
/// </summary>
public class InputConstants
{
    // current simplified gesture phase, (Began, Moved, Ended only)
    public TouchPhase currentPhase;

    // computed current gesture power (0 when currentPhase is Ended)
    public int gesturePower;
    // computed current gesture angle offset (0 when currentPhase is Ended)
    public float gestureZAngleOffset;

    // current gesture first touch (Vector3.zero when currentPhase is Ended)
    public Vector3 gestureStartPosition;
    // computed current gesture delta from first touch (Vector3.zero when currentPhase is Ended)
    public Vector3 gestureDelta;
    // current gesture position
    public Vector3 gesturePosition;

    public Vector3 cameraOffset;

    public InputConstants() 
    {
        currentPhase = TouchPhase.Ended;
        gestureStartPosition = Vector3.zero;
        gestureDelta = Vector3.zero;
        gesturePower = 0;
        gestureZAngleOffset = 0;
        cameraOffset = Vector3.zero;
    }

    private InputConstants(
        TouchPhase currentPhase, 
        int gesturePower, 
        float gestureZAngleOffset, 
        Vector3 gestureStartPosition, 
        Vector3 gestureDelta, 
        Vector3 gesturePosition, 
        Vector3 cameraOffset)
    {
        this.currentPhase = currentPhase;
        this.gesturePower = gesturePower;
        this.gestureZAngleOffset = gestureZAngleOffset;
        this.gestureStartPosition = gestureStartPosition;
        this.gestureDelta = gestureDelta;
        this.gesturePosition = gesturePosition;
        this.cameraOffset = cameraOffset;
    }

    public InputConstants copy()
    {
        return new InputConstants(
            currentPhase, 
            gesturePower, 
            gestureZAngleOffset, 
            gestureStartPosition, 
            gestureDelta, 
            gesturePosition, 
            cameraOffset);
    }
}

public class InputController : MonoBehaviour
{
    // event: a list of methods to perform, can use +=, -= to add and remove.
    // in this case, each event only allows methods with type GestureEventDelegate,
    // then calling the event later on in the script executes all methods in the list
    // with the parameters I pass to it. Similar to JS callbacks.
    public event GestureEventDelegate whenUpdated;
    public event GestureEventDelegate whenEnded;


    // pixels dragged in the y to increase power one step
    public int unitsYPerPower;
    // degrees to increase for each pixel dragged in the x
    public float degreesPerUnitX;

    // current state of the input
    private InputConstants currentState;

    // scalar factor * 100 to move camera when peeking around during flight
    public float peekSensitivity;

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
        if (currentState.currentPhase == TouchPhase.Ended)
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
        currentState.currentPhase = TouchPhase.Began;
        currentState.gestureStartPosition = startPosition;
        currentState.gesturePosition = startPosition;

        UpdateGestureProperties(startPosition);
    }

    void OnGestureDrag(Vector3 currentPosition)
    {
        currentState.currentPhase = TouchPhase.Moved;
        currentState.gestureDelta = currentState.gestureStartPosition - currentPosition;
        currentState.gesturePosition = currentPosition;

        UpdateGestureProperties(currentPosition);
    }

    void OnGestureRelease()
    {
        whenEnded(currentState.copy());

        ResetFields();
    }

    private void ResetFields()
    {
        currentState = new InputConstants();
    }

    private void UpdateGestureProperties(Vector3 position)
    {
        currentState.gesturePower = (int)Mathf.Clamp(currentState.gestureDelta.y / unitsYPerPower + 2, 0, 3);
        currentState.gestureZAngleOffset = Mathf.Clamp(degreesPerUnitX * currentState.gestureDelta.x, -180, 180);
        currentState.cameraOffset = currentState.gestureDelta * Mathf.Clamp(peekSensitivity / 100, 0.01f, 2.5f);

        whenUpdated(currentState.copy());
    }
}
