using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A utility that loads scenes
/// </summary>
public class SceneLoader : MonoBehaviour
{

    /// <summary>
    /// Loads the main scene
    /// Called from the inspector by Button.OnClick lists
    /// </summary>
    public void LoadMainScene()
    {
        LoadScene("Main");
    }

    /// <summary>
    /// Loads the title scene
    /// Called from the inspector by Button.OnClick lists
    /// </summary>
    public void LoadTitleScene()
    {
        LoadScene("Title");
    }
    /// <summary>
    /// Loads the scene with the provided name
    /// Called from the inspector by Button.OnClick
    /// </summary>
    /// <param name="name">name of the scene to load</param>
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("Settings");
    }

}
