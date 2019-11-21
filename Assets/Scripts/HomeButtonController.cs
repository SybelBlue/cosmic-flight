using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonController : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Title");
    }

}
