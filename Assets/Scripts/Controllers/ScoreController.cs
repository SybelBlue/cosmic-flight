using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that calculates the score at the end of each level based on
/// number of flights, and asteroids terraformed
/// </summary>
public class ScoreController : MonoBehaviour
{
	public string scoreStr;
	public int score;
	public Text displayer;
	
	/// <summary>
    /// calculates the score based on number of flights and asteroid counts
	/// calculation varies based on number of asteroids in the level
    /// </summary>
    internal void SetScore(int asteroidCounts, int launchCounts)
    {
		if (asteroidCounts == 0) {
			score = 500 - (launchCounts-1) * 5;
		}
        else if (asteroidCounts == 1) {
			score = 500 - (launchCounts-1) * 5 + asteroidCounts * 10;
		}
		else if (asteroidCounts == 2) {
			score = 500 - (launchCounts-1) * 6 + asteroidCounts * 10;
		}
		else {
			score = 500 - (launchCounts-1) * 8 + asteroidCounts * 12;
		}
		if (score >= 485)
			scoreStr = "Your Score: 3/3";
		else if (score >= 470 && score < 485)
			scoreStr = "Your Score: 2/3";
		else
			scoreStr = "Your Score: 1/3";
		displayer.text = scoreStr;
    }
}
