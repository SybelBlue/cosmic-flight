using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonController : MonoBehaviour
{

    /// <summary>
    /// The object that holds all level data for all levels.
    /// Usually only needed to be non-null in the Title Scene
    /// </summary>
    public GameObject levelData;

    /// <summary>
    /// Called from the inspector by Button.OnClick lists
    /// </summary>
    public void OnClick()
    {
        if (levelData != null)
        {
            DontDestroyOnLoad(levelData);
        }

        SceneManager.LoadScene("Main");
    }

}
