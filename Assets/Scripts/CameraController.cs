using UnityEngine;

/// <summary>
/// An enum that encodes all behavioral states the camera can be in
/// </summary>
public enum CameraMode
{
    Neutral, FollowRocket, StayPut
}

public class CameraController : MonoBehaviour
{

    public GameController gameController;
    public CameraMode mode;

    public Vector3 neutralPosition;
    public float neutralSize;
    public float followSize;

    public Vector3 offsetFromCenter;

    public float mapX, mapY;
    
    private float minX, maxX, minY, maxY;

    private Camera camera;

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

    internal bool InMapBounds(Vector3 position)
    {
        // Calculations assume map is position at the origin
        var minX = -mapX / 2.0f;
        var maxX = mapX / 2.0f;
        var minY = -mapY / 2.0f;
        var maxY = mapY / 2.0f;
        return minX <= position.x && position.x <= maxX && minY <= position.y && position.y <= maxY;
    }
}
