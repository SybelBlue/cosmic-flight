using UnityEngine;
using UnityEngine.EventSystems;

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
/// <param name="constants">the current set of constants for this input</param>
public delegate void GestureEventDelegate(InputConstants constants);

/// <summary>
/// A class which only serves to store some values and pass them safely.
/// Contains the full state of an input contoller, including many computed values
/// </summary>
public class InputConstants
{
    /// <summary>
    /// current simplified gesture phase, (Began, Moved, Ended only)
    /// </summary>
    public TouchPhase currentPhase;

    /// <summary>
    /// computed current gesture power (0 when currentPhase is Ended)
    /// </summary>
    public int gesturePower;

    /// <summary>
    /// computed current gesture angle offset (0 when currentPhase is Ended)
    /// </summary>
    public float gestureZAngleOffset;

    /// <summary>
    /// current gesture first touch (Vector3.zero when currentPhase is Ended)
    /// </summary>    
    public Vector3 gestureStartPosition;
    
    /// <summary>
    /// computed current gesture delta from first touch (Vector3.zero when currentPhase is Ended)
    /// </summary>    
    public Vector3 gestureDelta;

    /// <summary>
    /// current gesture position
    /// </summary>    
    public Vector3 gesturePosition;

    /// <summary>
    /// computed current camera offset from neutral, typically for FollowMode
    /// </summary>
    public Vector3 cameraOffset;

    /// <summary>
    /// Constructs a new instance of the state of an InputController with default values
    /// </summary>
    public InputConstants() 
    {
        currentPhase = TouchPhase.Ended;
        gestureStartPosition = Vector3.zero;
        gestureDelta = Vector3.zero;
        gesturePower = 0;
        gestureZAngleOffset = 0;
        cameraOffset = Vector3.zero;
    }

    /// <summary>
    /// For internal use only, see InputConstants.copy()
    /// </summary>
    /// <param name="currentPhase"></param>
    /// <param name="gesturePower"></param>
    /// <param name="gestureZAngleOffset"></param>
    /// <param name="gestureStartPosition"></param>
    /// <param name="gestureDelta"></param>
    /// <param name="gesturePosition"></param>
    /// <param name="cameraOffset"></param>
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

    /// <summary>
    /// Returns a safely mutable copy of this object
    /// </summary>
    /// <returns>an identical copy of this</returns>
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

/// <summary>
/// The primary class of the file, does all interfacing with the UnityInput
/// system, and is fully self-contained. The whenUpdated and whenEnded fields
/// are events that allow other classes to add callbacks to capture user actions.
/// All methods of this class are private. All else are events or public fields to 
/// be filled or viewed through the Unity Inspector.
/// </summary>
public class InputController : MonoBehaviour
{
    // event: a list of methods to perform, can use +=, -= to add and remove.
    // in this case, each event only allows methods with type GestureEventDelegate,
    // then calling the event later on in the script executes all methods in the list
    // with the parameters I pass to it. Similar to JS callbacks.
    // Take a look at the GameController constructor to see this in action.
    /// <summary>
    /// Called when the state of this object is updated, when a gesture is captured
    /// </summary>
    public event GestureEventDelegate whenUpdated;
    /// <summary>
    /// Called when a captured gesture ends
    /// </summary>
    public event GestureEventDelegate whenEnded;

    /// <summary>
    /// The "rings" to display when a user is aiming the rocket
    /// </summary>
    public GameObject rings;
    /// <summary>
    /// True when rings are active, false otherwise
    /// </summary>
    public bool displayRings;

    /// <summary>
    /// When false, all gesture inputs from all sources are muted 
    /// (UI elements and buttons remain unaffected)
    /// </summary>
    public bool acceptingInputs;

    /// <summary>
    /// pixels dragged in the y to increase power one step
    /// </summary>
    public int unitsYPerPower;
    /// <summary>
    /// degrees to increase for each pixel dragged in the x
    /// </summary>
    public float degreesPerUnitX;

    /// <summary>
    /// scalar factor * 100 to move camera when peeking around during flight
    /// </summary>
    public float peekSensitivity;

    /// <summary>
    /// current state of the input controller, computed and otherwise
    /// </summary>
    private InputConstants state;

    /// <summary>
    /// Initializes the controller to accept inputs
    /// </summary>
    private void Start()
    {
        acceptingInputs = true;
        ResetFields();
    }

    // Update is called once per frame
    private void Update()
    {
        // https://answers.unity.com/questions/784617/how-do-i-block-touch-events-from-propagating-throu.html
        if (!acceptingInputs || EventSystem.current.IsPointerOverGameObject()) return;

        CheckForTouches();

        CheckForClicks();
    }

    /// <summary>
    /// Turns touch data into internal Gesture calls
    /// </summary>
    private void CheckForTouches()
    {
        // taken from https://docs.unity3d.com/Manual/MobileInput.html   
        if (Input.touches.Length == 0) return; // if no touches, then exit

        Touch touch = Input.touches[0];
        if (state.currentPhase == TouchPhase.Ended)
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

    /// <summary>
    /// Turns click and drag data into internal Gesture calls
    /// </summary>
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

    /// <summary>
    /// Called when a gesture begins through any input
    /// </summary>
    /// <param name="startPosition">the location of the start of the gesture</param>
    private void OnGestureStart(Vector3 startPosition)
    {
        // display rings around rocket, display touch point
        state.currentPhase = TouchPhase.Began;
        state.gestureStartPosition = startPosition;
        state.gesturePosition = startPosition;

        if (displayRings)
        {
            rings.SetActive(true);
            rings.transform.position = startPosition + unitsYPerPower * 2 * Vector3.up;
            rings.transform.localScale = (2f * unitsYPerPower) / 100f * new Vector3(1, 1);
        }

        UpdateGestureProperties(startPosition);
    }

    /// <summary>
    /// Called when a gesture continues but did not end or begin in this update
    /// </summary>
    /// <param name="currentPosition">the current position of the gesture</param>
    private void OnGestureDrag(Vector3 currentPosition)
    {
        state.currentPhase = TouchPhase.Moved;
        state.gestureDelta = state.gestureStartPosition - currentPosition;
        state.gesturePosition = currentPosition;

        UpdateGestureProperties(currentPosition);
    }

    /// <summary>
    /// Called when a gesture ends normally
    /// </summary>
    private void OnGestureRelease()
    {
        whenEnded(state.copy());

        ResetFields();
    }

    /// <summary>
    /// Refreshes the state and resets the rings
    /// </summary>
    private void ResetFields()
    {
        state = new InputConstants();

        rings.transform.position = Vector3.zero;
        rings.transform.localScale = new Vector3(1, 1);
        rings.SetActive(false);
    }

    /// <summary>
    /// Re-computes the computed properties of state based on the latest position of the gesture input
    /// </summary>
    /// <param name="position">the newest posiiton of a gesture</param>
    private void UpdateGestureProperties(Vector3 position)
    {
        int oldPower = state.gesturePower;
        state.gesturePower = (int)Mathf.Clamp(state.gestureDelta.y / unitsYPerPower + 2, 0, 3);

        if (oldPower != state.gesturePower && displayRings)
        {
            Handheld.Vibrate();
        }

        state.gestureZAngleOffset = Mathf.Clamp(degreesPerUnitX * state.gestureDelta.x * -1 , -360, 360);
        state.cameraOffset = state.gestureDelta * Mathf.Clamp(peekSensitivity / 100, 0.01f, 2.5f);

        whenUpdated(state.copy());
    }
}
