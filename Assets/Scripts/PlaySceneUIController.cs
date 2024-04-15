using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySceneUIController : MonoBehaviour
{

    public AudioSource BGmusic;
    public AudioSource SFXmusic;
    public Slider bgSlider;
    public Slider sfxSlider;

    //Singleton pattern
    private static PlaySceneUIController _instance;
    public static PlaySceneUIController Instance { get { return _instance; } }


    void Awake(){
        _instance = this;
    }

    void Start(){
        if (PlayerPrefs.HasKey("BGVolume")){
            float savedVolume = PlayerPrefs.GetFloat("BGVolume", 1f);
            BGmusic.volume = savedVolume;
            bgSlider.value = savedVolume;
        }else{
            BGmusic.volume = 1f;
            bgSlider.value = 1f;
        }

        if (PlayerPrefs.HasKey("SFXVolume")){
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            SFXmusic.volume = savedSFXVolume;
            sfxSlider.value = savedSFXVolume;
        }else{
            SFXmusic.volume = 1f;
            sfxSlider.value = 1f;
        }
    }

    public void SetUIActive(GameObject ui)
    {   
        ui.SetActive(true);
    }

    public void SetUINotActive(GameObject ui)
    {   
        ui.SetActive(false);
    }


    public void SetVolume(float volume)
    {
        if(BGmusic != null)
        {
            BGmusic.volume = volume;
        }
        PlayerPrefs.SetFloat("BGVolume", volume); // Save the volume level to PlayerPrefs
    }

}
