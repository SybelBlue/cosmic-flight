using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public Sprite soundOn;
    public Sprite soundOff;
    public SpriteRenderer renderer;

    public void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void UpdateSound()
    {
        if (renderer.sprite.Equals(soundOn))
        {
            renderer.sprite = soundOff;
            AudioListener.pause = true;
        } else
        {
            renderer.sprite = soundOn;
            AudioListener.pause = false;
        }
    }
}
