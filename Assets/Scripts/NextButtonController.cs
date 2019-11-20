using UnityEngine;
using UnityEngine.SceneManagement;

public class NextButtonController : MonoBehaviour
{
	public Sprite[] imageArray;
	public SpriteRenderer fieldRenderer;
	public int currentImage = 0;

    public void OnClick(int gotoImage) // +1 for Next, -1 for Previous
    {
		if (currentImage < imageArray.Length) {
			currentImage += gotoImage;
			fieldRenderer.sprite = imageArray[currentImage];
		}
	}
}
