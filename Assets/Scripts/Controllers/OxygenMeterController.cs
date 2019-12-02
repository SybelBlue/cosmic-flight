using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// An enum that encodes all the modes of the oxygen meter
/// </summary>
public enum OxygenMode
{
    Safe, Flying, Landed, Paused
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
        target = GetTarget();
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

        label.text = string.Format("O2: {0}%", Mathf.RoundToInt(mainSlider.value));
        label.color = Color.Lerp(label.color, GetLabelColor(mainSlider.value / mainSlider.maxValue), 0.4f);
    }

    /// <summary>
    /// Gets the color of a label based on how full the meter
    /// is from 0.0 to 1.0
    /// </summary>
    /// <param name="value">fill ratio in [0.0, 1.0]</param>
    /// <returns>label color</returns>
    private Color GetLabelColor(float value)
    {
        if (value > 0.75f) return highO2Color;
        if (value > 0.3f) return mediumO2Color;
        return lowO2Color;
    }

    /// <summary>
    /// Takes an oxygen mode and returns the target value of
    /// the oxygen slider to smoothly slide to after one update.
    /// </summary>
    /// <returns>the target value in this mode after one update</returns>
    private float GetTarget()
    {
        switch (mode)
        {
            case OxygenMode.Landed:
                return Mathf.Max(0, target - asteroidLandedBurnRate / 100f);
            case OxygenMode.Flying:
                return Mathf.Max(0, target - flyBurnRate / 100f);
            case OxygenMode.Safe:
                return 100;
            case OxygenMode.Paused:
                return getOxygenLevel();
            default:
                Debug.LogError("O2 Mode Not Recognized!");
                return 100;
        }
        
    }

    /// <summary>
    /// Deducts O2 for claiming a world
    /// </summary>
    internal void ClaimAsteroid()
    {
        target -= claimCost;
    }

    private float getOxygenLevel()
    {
        return redSlider.value;
    }
}
