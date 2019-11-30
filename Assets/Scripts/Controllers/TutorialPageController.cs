﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
	public Sprite[] imageArray;
	public int page = 0;
    public SpriteRenderer fieldRenderer;
	public string[] instructions;
    public Text displayer; 

    public void Start()
    {
        fieldRenderer.sprite = imageArray[page];
		displayer.text = instructions[page];
    }

    public void OnClick(int gotoPage)
    {
        page += gotoPage;
		if (page < 0 || page >= imageArray.Length){
			SceneManager.LoadScene("Title");
			return;
		}
        fieldRenderer.sprite = imageArray[page];
		displayer.text = instructions[page];
    }
}