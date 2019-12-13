using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// A class that defines the behaviors of the buttons on the about page
/// </summary>
public class BlackHoleInfoController : MonoBehaviour
{
    public string[] aboutPages;
    public int page;
    public Text displayer; 

    public void Start()
    {
        displayer.text = aboutPages[page];
    }

    public void OnClick(int change)
    {
        page += change;

        if (page < 0 || page > 2) // the size of the array is 3
        {
            SceneManager.LoadScene("Title");
        }
        else
        {
            displayer.text = aboutPages[page];
        }

    }
}
