using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelHomeButtonController : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Title");
    }
}
