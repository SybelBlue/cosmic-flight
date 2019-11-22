using UnityEngine;
using UnityEngine.SceneManagement;


public class AboutButtonController : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("About");
    }
}
