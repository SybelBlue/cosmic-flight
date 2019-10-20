using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButtonController : MonoBehaviour
{ 
    public void OnClick()
    {
        SceneManager.LoadScene("Main");
    }

}
