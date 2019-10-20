using UnityEngine;

public class LevelData : MonoBehaviour
{
    public LevelConfig[] levels;

    public int levelNumber;

    public float warningThreshold;

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
