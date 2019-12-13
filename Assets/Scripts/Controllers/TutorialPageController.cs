using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// A class that controls the NextButton and PreviousButton on the Tutorial Scene
/// </summary>
public class TutorialPageController : MonoBehaviour
{
	public GameObject[] tutorialArray;
	public int page = 0;
    
	/// <summary>
    /// loads the first page of the Tutorial
    /// </summary>
    public void Start()
    {
		tutorialArray[page].SetActive(true);
    }

	/// <summary>
    /// loads page by going forward or backward by 1
    /// </summary>
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