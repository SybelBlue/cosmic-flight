using UnityEngine;

public class RocketFireController : MonoBehaviour
{
    public SpriteRenderer renderer;
    public bool aiming;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        aiming = false;
    }


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
