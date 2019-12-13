using UnityEngine;

/// <summary>
/// A class that defines the unique behaviors of an asteroid prefab's flag
/// </summary>
public class FlagController : MonoBehaviour
{

    /// <summary>
    /// True when the flag is raising or raised, false otherwise
    /// </summary>
    public bool isRaised;

    /// <summary>
    /// Sets up this object to be hidden and small
    /// </summary>
    void Start()
    {
        isRaised = false;
        transform.localScale = new Vector3(0.05f, 0.05f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Grows the flag's localScale, capped at 1, when isRaised is true,
    /// shrinks it and hides it otherwise
    /// </summary>
    void Update()
    {
        if (isRaised)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1, 1), 0.2f);
        }
        else
        {
            if (!gameObject.activeSelf) return;

            if (transform.localScale.sqrMagnitude < 0.1f)
            {
                gameObject.SetActive(false);
                return;
            }

            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0), 0.2f);
        }
    }

    /// <summary>
    /// Sets up the flag to be raised, fires the raise animation
    /// </summary>
    /// <param name="raised"></param>
    internal void SetRaised(bool raised)
    {
        isRaised = raised;
        gameObject.SetActive(true);
    }
}
