using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
    public Text statsText;
    private Rect rect;

    void Start()
    {
        ResetFields(); 
    }

    public void ResetFields()
    {
        rect = GetComponent<RectTransform>().rect;
        gameObject.SetActive(false);
        statsText.text = "Stats Text\nStats Text 2";
    }


    public void DisplayShotStatistics(InputConstants constants, float canvasWidth, float canvasHeight)
    {
        Vector3 position = constants.gesturePosition;
        int gesturePower = constants.gesturePower;
        float angleOffset = constants.gestureZAngleOffset;

        gameObject.SetActive(true);
        // TODO: maybe later add invisible game object at touch position, child statsGObject to it here
        transform.position = position + new Vector3(0, 20, 0);

        var newPos = transform.position;

        // weird units here, hence the *2 and the *4, though I'm not sure why...

        // clamp the object so that it can't be offscreen in the x direction...
        float halfWidth = rect.width / 2;
        newPos.x = Mathf.Clamp(newPos.x, halfWidth, canvasWidth - halfWidth * 2);

        // ... or the y direction
        float halfHeight = rect.height / 2;
        newPos.y = Mathf.Clamp(newPos.y, halfHeight, canvasHeight - 4 * halfHeight);

        transform.position = newPos;

        // set the text
        statsText.text = (constants.gesturePower > 0) ?
            string.Format("Power Level: {0}\nAngle: {1}",
                constants.gesturePower,
                (int)constants.gestureZAngleOffset
            ) :
            "Cancel\nShot";
    }
}
