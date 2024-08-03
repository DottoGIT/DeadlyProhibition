using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("Volume", 0.8f);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", slider.value);
    }

    public void changeValue()
    {
        PlayerPrefs.SetFloat("Volume", slider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", slider.value);
    }
}
