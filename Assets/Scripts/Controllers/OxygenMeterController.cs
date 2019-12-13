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

    /// <summary>
    /// The GameContoller instance for the current level
    /// </summary>
    public GameController gameController;

    /// <summary>
    /// The text label next to the sliders
    /// </summary>
    public Text label;

    /// <summary>
    /// The current mode of the oxygen meter
    /// </summary>
    public OxygenMode mode;

    /// <summary>
    /// The background panel that flashes when
    /// the oxygen level is less than 15%
    /// </summary>
    public GameObject flashingPanel;

    /// <summary>
    /// The radius in which this object can jitter
    /// </summary>
    public Vector3 jitterValue;

    /// <summary>
    /// The rate at which flight burns oxygen
    /// </summary>
    public float flyBurnRate;

    /// <summary>
    /// The rate at which being landed on an asteroid burns oxygen
    /// </summary>
    public float asteroidLandedBurnRate;

    /// <summary>
    /// The one-time charge for terraforming an asteroid
    /// </summary>
    public float claimCost;

    /// <summary>
    /// The current amount of oxygen in the tanks
    /// </summary>
    public float target;

    /// <summary>
    /// The decay speed of the red slider per frame
    /// </summary>
    public float decreasePerTick;
    
    /// <summary>
    /// The number of frames it takes for the background panel
    /// to toggle on or off when the oxygen level is under 15%
    /// </summary>
    public int framesPerFlash;

    /// <summary>
    /// The color of the label text when O2 is over 70%
    /// </summary>
    public Color highO2Color;

    /// <summary>
    /// The color of the label text when O2 is over 30% 
    /// and under 70%
    /// </summary>
    public Color mediumO2Color;

    /// <summary>
    /// The color of the label text when O2 is over 30%
    /// </summary>
    public Color lowO2Color;

    /// <summary>
    /// The red background slider behind the main slider 
    /// that updates slowly to the current value
    /// </summary>
    public Slider redSlider;

    /// <summary>
    /// The main slider that indicates the oxygen level
    /// </summary>
    public Slider mainSlider;

    /// <summary>
    /// If true, the label text will be updated with the oxygen
    /// </summary>
    public bool displayPercent;

    /// <summary>
    /// The number of frames since the background panel
    /// was toggled on or off
    /// </summary>
    private int framesSinceToggle;

    /// <summary>
    /// The starting position of the oxygen meter,
    /// reference point for the jitter animation
    /// </summary>
    private Vector3 startingPosition;

    /// <summary>
    /// Sets up the constants for animation 
    /// </summary>
    private void Start()
    {
        startingPosition = transform.position;
        displayPercent = false;
    }

    /// <summary>
    /// Animates the main slider, the red slider, the background, and the jitter,
    /// and reports to the gameController when there is no more oxygen left
    /// </summary>
    void Update()
    {
        // do the slider updates
        target = GetTarget();
        mainSlider.value = Mathf.Lerp(mainSlider.value,
            target,
            mode == OxygenMode.Safe ? 0.2f : 0.9f);
        redSlider.value = Mathf.Max(mainSlider.value + (mode == OxygenMode.Safe ? 0 : 5), redSlider.value - decreasePerTick);

        // update the label
        if (displayPercent)
        {
            label.text = string.Format("O2: {0}%", Mathf.RoundToInt(mainSlider.value));
        }

        label.color = Color.Lerp(label.color, GetLabelColor(mainSlider.value / mainSlider.maxValue), 0.4f);

        // do the critical levels animation
        if (target <= 30 && mode != OxygenMode.Safe)
        {
            framesSinceToggle++;
            
            if (framesSinceToggle % 2 == 0)
            {
                jitterValue = Random.insideUnitCircle * (target <= 15 ? 5 : 3);
                transform.position = startingPosition + jitterValue;
            }

            if (target <= 15 && framesSinceToggle >= framesPerFlash)
            {
                flashingPanel.SetActive(!flashingPanel.activeSelf);
                framesSinceToggle = 0;
            }
        } 
        else
        {
            flashingPanel.SetActive(false);
            transform.position = startingPosition;
            framesSinceToggle = 0;
        }

        // warn the gameController
        if (mainSlider.value < 0.2f)
        {
            gameController.OutOfOxygen();
        }
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
                return redSlider.value;
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
}
