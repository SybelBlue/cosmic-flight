using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleInfoController : MonoBehaviour
{
    public string[] aboutPages;
    public int page = 0;
    public Text displayer; 

    public void Start()
    {
        aboutPages[0] = "a";
        aboutPages[1] = "b";
        aboutPages[2] = "c";

        displayer.text = aboutPages[page];
    }

    public void OnClick(int change)
    {
        page += change;
        displayer.text = aboutPages[page];
    }
}
