using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
