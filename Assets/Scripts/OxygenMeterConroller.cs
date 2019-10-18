using System;
using UnityEngine;
using UnityEngine.UI;

public enum OxygenMode
{
    Safe, Flying, Landed
}

public class OxygenMeterConroller : MonoBehaviour
{
    public OxygenMode mode;
    public float flyBurnRate, asteroidLandedBurnRate, target;
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        target = GetTarget(mode);
        slider.value = Mathf.Lerp(slider.value, target, 0.5f);
    }

    private float GetTarget(OxygenMode mode)
    {
        switch (mode)
        {
            case OxygenMode.Landed:
                return target - asteroidLandedBurnRate / 100f;
            case OxygenMode.Flying:
                return target - flyBurnRate / 100f;
            case OxygenMode.Safe:
                return 100;
            default:
                Debug.LogError("O2 Mode Not Recognized!");
                return 100;
        }
        
    }

    /// <summary>
    /// Sets the O2 meter to fill level between 0 and 100.
    /// </summary>
    /// <param name="f"> Between 0 and 1 </param>
    internal void SetSliderValue(float f)
    {
        target = f;
    }


}
