using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialPageController : MonoBehaviour
{
	//public Sprite[] imageArray;
	public GameObject[] tutorialArray;
	public int page = 0;
    //public SpriteRenderer fieldRenderer;
	//public string[] instructions;
    //public Text displayer; 

    public void Start()
    {
        //fieldRenderer.sprite = imageArray[page];
		tutorialArray[page].SetActive(true);
		//displayer.text = instructions[page];
    }

    public void ChangePageNumberBy(int gotoPage)
    {
        page += gotoPage;
		Debug.Log(page);
		if (page < 0 || page >= tutorialArray.Length){
			SceneManager.LoadScene("Title");
			return;
		}
        //fieldRenderer.sprite = imageArray[page];
		//tutorialArray[page].SetActive(true);
		for (int i = 0; i < tutorialArray.Length; i++) {
			tutorialArray[i].SetActive(i == page);
		}
		//displayer.text = instructions[page];
    }
}