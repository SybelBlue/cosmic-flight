using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonController : MonoBehaviour
{

    public GameObject levelData;

    public void OnClick()
    {
        DontDestroyOnLoad(levelData);
        SceneManager.LoadScene("Main");
    }

}
