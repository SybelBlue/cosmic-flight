using UnityEngine;

/// <summary>
/// A class that defines the unique behaviors of an rocket's trailing fire
/// </summary>
public class RocketFireController : MonoBehaviour
{

    /// <summary>
    /// This objects sprite renderer
    /// </summary>
    private SpriteRenderer renderer;

    /// <summary>
    /// True when in aiming mode, false otherwise
    /// </summary>
    public bool aiming;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        aiming = false;
    }

    /// <summary>
    /// Sets the aiming mode and 
    /// sets transparency and activeSelf
    /// </summary>
    /// <param name="aiming">true when aiming, false otherwise</param>
    internal void SetAiming(bool aiming)
    {
        this.aiming = aiming;
        var oldColor = renderer.color;
        oldColor.a = aiming ? 0.5f : 1.0f;
        renderer.color = oldColor;
        if (aiming)
        {
            gameObject.SetActive(true);
        }
    }
}
