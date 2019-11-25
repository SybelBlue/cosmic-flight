using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    /// <summary>
    /// Called from the inspector by Button.OnClick lists
    /// </summary>
    public void LoadMainScene()
    {
        LoadScene("Main");
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

}
