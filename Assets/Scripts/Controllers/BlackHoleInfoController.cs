using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// A class that controls the NextButton and PreviousButton on the About Scene
/// </summary>
public class BlackHoleInfoController : MonoBehaviour
{
    public string[] aboutPages;
    public int page;
    public Text displayer; 

	/// <summary>
    /// loads the first page of the Tutorial
    /// </summary>
    public void Start()
    {
        displayer.text = aboutPages[page];
    }

	/// <summary>
    /// loads page by going forward or backward by 1
    /// </summary>
    public void OnClick(int change)
    {
        page += change;

        if (page < 0 || page > 1) // the size of the array is 2
        {
            SceneManager.LoadScene("Title");
        }
        else
        {
            displayer.text = aboutPages[page];
        }

    }
}
