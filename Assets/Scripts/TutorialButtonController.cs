﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButtonController : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("Tutorial");
    }

}