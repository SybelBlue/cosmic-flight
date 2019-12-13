using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that defines the behavior of the aiming stats display
/// (Depreciated)
/// </summary>
public class StatsController : MonoBehaviour
{
    /// <summary>
    /// The offset from the screen location of user touch
    /// </summary>
    public Vector3 touchOffset;

    /// <summary>
    /// The text object to display stats on
    /// </summary>
    public Text statsText;

    /// <summary>
    /// Resets this object
    /// </summary>
    void Start()
    {
        ResetFields(); 
    }

    /// <summary>
    /// Sets the fields to their inspector state
    /// </summary>
    public void ResetFields()
    {
        gameObject.SetActive(false);
        statsText.text = "Stats Text\nStats Text 2";
    }

    /// <summary>
    /// Activates the shot statistics text box.
    /// TODO: make the text box not capable of being dragged off-screen
    /// </summary>
    /// <param name="constants">the data container to draw from</param>
    /// <param name="canvasWidth">used to calculate bounds</param>
    /// <param name="canvasHeight">used to calculate bounds</param>
    public void DisplayShotStatistics(InputConstants constants, float canvasWidth, float canvasHeight)
    {
        Vector3 position = constants.gesturePosition;
        int gesturePower = constants.gesturePower;
        float angleOffset = constants.gestureZAngleOffset;

        gameObject.SetActive(true);
        // TODO: maybe later add invisible game object at touch position, child statsGObject to it here
        transform.position = position + touchOffset;

        // took this stuff out cause scaling issues were being sad
        //var newPos = transform.position;

        // weird units here, hence the *2 and the *4, though I'm not sure why...

        // clamp the object so that it can't be offscreen in the x direction...
        //float halfWidth = rect.width / 2;
        //newPos.x = Mathf.Clamp(newPos.x, halfWidth, canvasWidth - halfWidth * 2);

        // ... or the y direction
        //float halfHeight = rect.height / 2;
        //newPos.y = Mathf.Clamp(newPos.y, halfHeight, canvasHeight - 4 * halfHeight);

        //transform.position = newPos;

        // set the text
        statsText.text = (constants.gesturePower > 0) ?
            string.Format("Power Level: {0}\nAngle: {1}",
                gesturePower,
                (int)angleOffset
            ) :
            "Cancel\nShot";
    }
}
