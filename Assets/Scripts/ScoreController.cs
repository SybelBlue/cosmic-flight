using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
	public string scoreStr;
	public Text displayer;
	

    internal void SetScore(int launchCounts)
    {
        scoreStr = "Your Score: " + (500 - （launchCounts-1） * 5);
		displayer.text = scoreStr;
    }
}
