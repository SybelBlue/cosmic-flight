using UnityEngine;

/// <summary>
/// A class attachable to a GameObject designed to store all
/// LevelConfig instances for the whole game
/// </summary>
public class LevelManagerController : MonoBehaviour
{

    public LevelConfig[] levels;

    /// <summary>
    /// The current level number
    /// </summary>
    public int levelNumber;

    /// <summary>
    /// The distance at which level configs will warn in the inspector
    /// if the rocket, the black hole, or any of the planets are too close
    /// </summary>
    public float warningThreshold;

    /// <summary>
    /// The current level's rocket starting position (computed)
    /// </summary>
    public Vector3 rocketStartingPosition
    {
        get
        {
            return levels[levelNumber].rocketStartingPosition;
        }
    }

    /// <summary>
    /// The current level's first planet position (computed)
    /// </summary>
    public Vector3 planetPosition
    {
        get
        {
            return levels[levelNumber].planetPosition;
        }
    }

    /// <summary>
    /// The current level's asteroid starting positions (computed)
    /// </summary>
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
        if (levels == null) return;
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == null)
            {
                continue;
            }

            levels[i].levelNumber = i;
            levels[i].warningThreshold = warningThreshold;
            levels[i].OnValidate();
        }
    }
}
