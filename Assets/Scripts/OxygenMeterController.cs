using System;
using UnityEngine;
using UnityEngine.UI;

public enum OxygenMode
{
    Safe, Flying, Landed
}

public class OxygenMeterController : MonoBehaviour
{
    public GameController gameController;
    public Text label;
    public OxygenMode mode;
    public float flyBurnRate, asteroidLandedBurnRate, claimCost, target;
    public Color highO2Color, mediumO2Color, lowO2Color;
    public Slider mainSlider, redSlider;

    void Update()
    {
        target = GetTarget(mode);
        mainSlider.value = Mathf.Lerp(mainSlider.value,
            target,
            mode == OxygenMode.Safe ? 0.2f : 0.9f);
        redSlider.value = Mathf.Lerp(redSlider.value,
            target,
            mode == OxygenMode.Safe ? 0.2f : 0.5f);

        if (mainSlider.value < 0.2f)
        {
            gameController.OutOfOxygen();
        }

        label.text = String.Format("O2: {0}%", Mathf.RoundToInt(mainSlider.value));
        label.color = Color.Lerp(label.color, GetLabelColor(mainSlider.value / mainSlider.maxValue), 0.4f);
    }

    private Color GetLabelColor(float value)
    {
        if (value > 0.75f) return highO2Color;
        if (value > 0.3f) return mediumO2Color;
        return lowO2Color;
    }

    private float GetTarget(OxygenMode mode)
    {
        switch (mode)
        {
            case OxygenMode.Landed:
                return Mathf.Max(0, target - asteroidLandedBurnRate / 100f);
            case OxygenMode.Flying:
                return Mathf.Max(0, target - flyBurnRate / 100f);
            case OxygenMode.Safe:
                return 100;
            default:
                Debug.LogError("O2 Mode Not Recognized!");
                return 100;
        }
        
    }

    internal void ClaimAsteroid()
    {
        target -= claimCost;
    }
}
