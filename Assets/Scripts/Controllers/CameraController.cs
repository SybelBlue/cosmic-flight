using UnityEngine;

/// <summary>
/// An enum that encodes all behavioral states the camera can be in
/// </summary>
public enum CameraMode
{
    Neutral, FollowRocket, StayPut, Wide
}

/// <summary>
/// A class that defines the unique behaviors of the main camera in the main scene
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// A reference to the GameController on this level
    /// </summary>
    public GameController gameController;

    /// <summary>
    /// The mode the camera should be in
    /// </summary>
    public CameraMode mode;

    /// <summary>
    /// The position the camera should rest in during neutral mode
    /// </summary>
    public Vector3 neutralPosition;
    /// <summary>
    /// The frame size of the camera during neutral mode
    /// </summary>
    public float neutralSize;
    /// <summary>
    /// The frame size of the camera during follow mode
    /// </summary>
    public float followSize;
    /// <summary>
    /// The frame size of the camera during wide mode
    /// </summary>
    public float wideSize;

    /// <summary>
    /// The offset of the camera from the focus point in the frame
    /// </summary>
    public Vector3 offsetFromCenter;

    /// <summary>
    /// Constants to keep camera in-bounds provided by answers found here
    /// https://answers.unity.com/questions/501893/calculating-2d-camera-bounds.html
    /// </summary>
    public float mapX, mapY;
    private float minX, maxX, minY, maxY;

    /// <summary>
    /// The camera script on this object
    /// </summary>
    private Camera camera;

    /// <summary>
    /// Sets up the basic constants for the camera
    /// </summary>
    private void Start()
    {
        mode = CameraMode.Neutral;
        camera = GetComponent<Camera>();

        // camera bounding code from 
        // https://answers.unity.com/questions/501893/calculating-2d-camera-bounds.html
        var vertExtent = camera.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = horzExtent - mapX / 2.0f;
        maxX = mapX / 2.0f - horzExtent;
        minY = vertExtent - mapY / 2.0f;
        maxY = mapY / 2.0f - vertExtent;
    }

    /// <summary>
    /// Adjusts the camera position depending on the mode
    /// </summary>
    private void Update()
    {
        Vector3 focus;
        float size;

       switch (mode)
        {
            case CameraMode.StayPut:
                return;
            case CameraMode.FollowRocket:
                focus = gameController.CurrentRocketPosition();
                size = followSize;
                break;
            case CameraMode.Neutral:
                focus = neutralPosition;
                size = neutralSize;
                break;
            case CameraMode.Wide:
                // 3/4s of the way between the black hole (at the origin) and the rocket
                focus = 3 * gameController.CurrentRocketPosition() / 4.0f;
                size = wideSize;
                break;
            default: // in case of future additions to CameraMode
                focus = neutralPosition;
                size = neutralSize;
                Debug.LogError("Mode not recognized!");
                break;
        }

        CenterOn(focus, size);
    }

    /// <summary>
    /// Centers the camera on position (x,y) with viewport diagonal size
    /// </summary>
    /// <param name="position">position to center on (z ignored)</param>
    /// <param name="size">new camera orthographicSize</param>
    private void CenterOn(Vector3 position, float size)
    {
        position += offsetFromCenter;
        position.z = transform.position.z;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = Vector3.Slerp(transform.position, position, 0.5f);
        camera.orthographicSize = 
            Mathf.Abs(camera.orthographicSize - size) > 1 ? 
                Mathf.Lerp(camera.orthographicSize, size, 0.5f) : 
                size;

        if ((transform.position - position).sqrMagnitude > 0.2f) return;

        transform.position = position;
    }
}
