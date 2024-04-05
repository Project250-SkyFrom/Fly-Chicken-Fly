using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mutable : MonoBehaviour
{
    public Slider bgSlider;
    public Slider sfxSlider;
    public AudioSource BGmusic;
    public AudioSource SFXmusic;

    void Awake()
    {
        float savedVolume = PlayerPrefs.GetFloat("BGVolume", 0.5f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        BGmusic.volume = savedVolume;
        if (bgSlider != null)
        {
            bgSlider.value = savedVolume;
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFXVolume;
        }
    }

    void Start()
    {
        if (bgSlider != null)
        {
            bgSlider.onValueChanged.AddListener(SetVolume);
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }


    public void SetVolume(float volume)
    {
        BGmusic.volume = volume;
        PlayerPrefs.SetFloat("BGVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXmusic.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    void OnDestroy()
    {
        if (bgSlider != null)
        {
            bgSlider.onValueChanged.RemoveListener(SetVolume);
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        }
    }
}
