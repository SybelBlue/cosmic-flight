using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
	public Sprite[] imageArray;
	public int currentImage = 0;
    public SpriteRenderer fieldRenderer;

    public void Start()
    {
        fieldRenderer.sprite = imageArray[currentImage];
    }

    public void OnClick(int gotoImage)
    {
        currentImage += gotoImage;
        fieldRenderer.sprite = imageArray[currentImage];
    }
}