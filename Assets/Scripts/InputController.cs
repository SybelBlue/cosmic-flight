using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{

    public GameController game;

    public GameObject statsGObject;
    private Text statsText;

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

        UpdateGestureProperties(startPosition);
    }

    void OnControlDrag(Vector3 currentPosition)
    {
        // change angle, change level
        currentPhase = TouchPhase.Moved;
        gestureDelta = gestureStartPosition - currentPosition;

        UpdateGestureProperties(currentPosition);
    }

    void OnControlRelease()
    {
        if (gesturePower > 0)
        {
            game.ShootRocket(gestureZAngleOffset, gesturePower);
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

        statsGObject.SetActive(false);
        if (statsText == null)
        {
            statsText = statsGObject.GetComponent<Text>();
        }
    }

    private void UpdateGestureProperties(Vector3 position)
    {
        gesturePower = (int)Mathf.Clamp(gestureDelta.y / unitsYPerPower + 2, 0, 3);
        gestureZAngleOffset = Mathf.Clamp(degreesPerUnitX * gestureDelta.x, -180, 180);

        game.AimRocketAtAngle(gestureZAngleOffset);

        statsGObject.SetActive(true);
        // TODO: maybe later add invisible game object at touch position, child statsGObject to it here
        statsGObject.transform.position = position + new Vector3(0, 20, 0);

        float halfWidth = statsGObject.GetComponent<RectTransform>().rect.width / 2;
        var oldPos = statsGObject.transform.position;
        //oldPos.x = Mathf.Clamp(statsGObject.transform.position.x, halfWidth, game.GetOverlayCanvasWidth() - halfWidth); //need to implement GetOverlayCanvasWidth
        statsGObject.transform.position = oldPos;

        statsText.text = (gesturePower > 0) ?
            string.Format("Power Level: {0}\nAngle: {1}", 
                gesturePower, 
                (int)gestureZAngleOffset
            ) :
            "Cancel\nShot";
    }
}
