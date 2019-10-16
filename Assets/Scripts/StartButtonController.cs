using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonController : MonoBehaviour
{
    
    public void OnClick()
    {
        SceneManager.LoadScene("Main");
    }

}
