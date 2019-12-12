using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialPageController : MonoBehaviour
{
	public GameObject[] tutorialArray;
	public int page = 0;
    
    public void Start()
    {
		tutorialArray[page].SetActive(true);
    }

    public void ChangePageNumberBy(int gotoPage)
    {
        page += gotoPage;
		Debug.Log(page);
		if (page < 0 || page >= tutorialArray.Length){
			SceneManager.LoadScene("Title");
			return;
		}

		for (int i = 0; i < tutorialArray.Length; i++) {
			tutorialArray[i].SetActive(i == page);
		}
    }
}