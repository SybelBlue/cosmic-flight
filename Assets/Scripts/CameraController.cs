using System;
using System.Collections;
using UnityEngine;

public enum CameraMode
{
    Neutral, FollowRocket, StayPut//, Cinematic
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

    //private Vector3 cinematicPosition;
    //private float cinematicSize;
    //private CameraMode lastMode;
    //private Action cinematicCallback;

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
            //case CameraMode.Cinematic:
            //    focus = cinematicPosition;
            //    size = cinematicSize;
            //    break;
            default:
                focus = neutralPosition;
                size = neutralSize;
                Debug.LogError("Mode not recognized!");
                break;
        }

        CenterOn(focus, size);
    }

    internal void CenterOn(Vector3 position, float size)
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

        //if (mode == CameraMode.Cinematic)
        //{
        //    mode = CameraMode.StayPut;
        //    StartCoroutine(CinematicPositionReached());
        //}
    }

    //internal void CinematicFocusOn(Vector3 position, float size, Action callback)
    //{
    //    lastMode = mode;
    //    mode = CameraMode.Cinematic;
    //    cinematicPosition = position;
    //    cinematicSize = size;
    //    cinematicCallback = callback;
    //    gameController.SetAllowInputs(false);
    //    gameController.Pause();
    //}

    //private IEnumerator CinematicPositionReached()
    //{
    //    yield return new WaitForSeconds(1);
    //    mode = lastMode;
    //    gameController.SetAllowInputs(true);
    //    gameController.Play();
    //    cinematicCallback();
    //}
}
