using UnityEngine;
using UnityEngine.UI;

public class CounterController : MonoBehaviour
{
    public Text label;

    public int value;

    internal void SetValue(int value)
    {
        this.value = value;
        label.text = string.Format("{0}", value);
    }

    internal void Increment()
    {
        SetValue(value + 1);
    }

    internal void Decrement()
    {
        SetValue(value - 1);
    }

    public static CounterController operator ++(CounterController controller)
    {
        controller.Increment();
        return controller;
    }

    public static CounterController operator --(CounterController controller)
    {
        controller.Decrement();
        return controller;
    }
}
