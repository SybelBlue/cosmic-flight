using UnityEngine;

/// <summary>
/// A class attachable to a GameObject designed to store all
/// LevelConfig instances for the whole game
/// </summary>
public class LevelData : MonoBehaviour
{
    public LevelConfig[] levels;

    public int levelNumber;

    public float warningThreshold;

    // computed properties, but getters nonetheless, all relative to the selected levelNumber
    public Vector3 rocketStartingPosition
    {
        get
        {
            return levels[levelNumber].rocketStartingPosition;
        }
    }
    public Vector3 planetPosition
    {
        get
        {
            return levels[levelNumber].planetPosition;
        }
    }

    public Vector3[] asteroidStartingPostions
    {
        get
        {
            return levels[levelNumber].asteroidStartingPostions;
        }
    }

    /// <summary>
    /// Propigates important properties down, auto-labels them with
    /// their level number, and then requests each to validate its own data.
    /// Called whenever a change is made to this object in the inspector.
    /// </summary>
    private void OnValidate()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == null)
            {
                levels[i] = ScriptableObject.CreateInstance<LevelConfig>();
            }

            levels[i].levelNumber = i;
            levels[i].warningThreshold = warningThreshold;
            levels[i].OnValidate();
        }
    }
}
