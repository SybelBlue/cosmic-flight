using UnityEngine;

/// <summary>
/// Used as a container for all data necessary to make a new level.
/// Contains extra bells and whistles to let unity do the hard part
/// and enable level creation via the inspector only. Not attachable
/// to a GameObject.
/// </summary>
[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelConfig", order = 1)]
public class LevelConfig : ScriptableObject
{

    public int levelNumber;

    public Vector3 rocketStartingPosition, planetPosition;
    public Vector3[] asteroidStartingPostions;

    public float warningThreshold;

	[TextArea]
	public string comments;

    /// <summary>
    /// Checks that inputted properties are not invalid parameters for a level.
    /// Called each time a value in the inspector is altered.
    /// </summary>
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
