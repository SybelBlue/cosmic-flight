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
        aboutPages[0] = "Your mission is to transform asteroids into habitable planets.\n\nBut be warned, there is a black hole making your mission harder… ";
        aboutPages[1] = "A black hole is a massive region of space that does not follow the laws of physics that we're used to. It has such strong gravity that it can curve the space around it.";
        aboutPages[2] = "The image of the black hole in Cosmic Flight is based on the image produced by Dr. Katie Bouman and the team she led.";

        displayer.text = aboutPages[page];
    }

    public void OnClick(int change)
    {
        page += change;
        displayer.text = aboutPages[page];
    }
}
