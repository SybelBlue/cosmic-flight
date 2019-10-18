using UnityEngine;
using UnityEngine.UI;

public class OxygenMeterConroller : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
    }

    /// <summary>
    /// Sets the O2 meter to fill level between 0 and 1.
    /// </summary>
    /// <param name="f"> Between 0 and 1 </param>
    internal void SetSliderValue(float f)
    {
        slider.value = f;
    }
}
