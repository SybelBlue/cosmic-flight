using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleInfoController : MonoBehaviour
{
    public string[] aboutPages;
    public int page;
    public Text displayer; 

    public void Start()
    {
        displayer.text = aboutPages[page];
    }

    public void OnClick(int change)
    {
        page += change;
        displayer.text = aboutPages[page];
    }
}
