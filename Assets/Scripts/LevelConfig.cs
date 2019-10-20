using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{

    public int levelNumber;

    public Vector3 rocketStartingPosition, planetPosition;
    public Vector3[] asteroidStartingPostions;

    public float warningThreshold;

    internal void OnValidate()
    {
        if ((rocketStartingPosition - planetPosition).magnitude < warningThreshold)
        {
            Debug.LogWarningFormat("Level {0} :: Rocket too close to planet starting position!", levelNumber);
        }

        if (asteroidStartingPostions == null) return;

        for (int i = 0; i < asteroidStartingPostions.Length; i++)
        {
            if ((rocketStartingPosition - asteroidStartingPostions[i]).magnitude < warningThreshold)
            {
                Debug.LogWarningFormat("Level {0} :: Rocket too close to asteroid starting position[{1}]: {2}!", levelNumber, i, asteroidStartingPostions[i]);
            }
        }
    }
}
