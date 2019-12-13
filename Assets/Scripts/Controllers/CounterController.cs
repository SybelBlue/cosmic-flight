using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that defines the unique behaviors of the in-level counter prefab
/// </summary>
public class CounterController : MonoBehaviour
{
    /// <summary>
    /// The text of this counter
    /// </summary>
    public Text label;

    /// <summary>
    /// The value of the counter
    /// </summary>
    public int value;

    /// <summary>
    /// Sets the value and updates the label text
    /// </summary>
    /// <param name="value"></param>
    internal void SetValue(int value)
    {
        this.value = value;
        label.text = string.Format("{0}", value);
    }

    /// <summary>
    /// Increments the value and updates the label text
    /// </summary>
    internal void Increment()
    {
        SetValue(value + 1);
    }

    /// <summary>
    /// Decrements the value and updates the label text
    /// </summary>
    internal void Decrement()
    {
        SetValue(value - 1);
    }

    /// <summary>
    /// Increments the value, updates the label text, 
    /// and returns the updated CounterController object
    /// </summary>
    public static CounterController operator ++(CounterController controller)
    {
        controller.Increment();
        return controller;
    }

    /// <summary>
    /// Decrements the value, updates the label text, 
    /// and returns the updated CounterController object
    /// </summary>
    public static CounterController operator --(CounterController controller)
    {
        controller.Decrement();
        return controller;
    }	
}
