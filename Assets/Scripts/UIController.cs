using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public AudioSource BGmusic;
    public AudioSource SFXmusic;
    public GameObject tonOn;
    public GameObject toffOff;
    public GameObject tonOff;
    public GameObject toffOn;
    public GameObject tutorial;
    public Slider bgSlider;
    public Slider sfxSlider;
    public GameObject blurLayer;
    public GameObject blurCanvas;

    //Singleton pattern
    private static UIController _instance;
    public static UIController Instance { get { return _instance; } }


    void Awake(){
        _instance = this;
    }

    void Start(){
        if (PlayerPrefs.HasKey("BGVolume")){
            float savedVolume = PlayerPrefs.GetFloat("BGVolume", 0.5f);
            BGmusic.volume = savedVolume;
            bgSlider.value = savedVolume;
        }else{
            BGmusic.volume = 0.5f;
            bgSlider.value = 0.5f;
        }

        if (PlayerPrefs.HasKey("SFXVolume")){
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
            SFXmusic.volume = savedSFXVolume;
            sfxSlider.value = savedSFXVolume;
        }else{
            SFXmusic.volume = 0.5f;
            sfxSlider.value = 0.5f;
        }

        //tutorial
        if (PlayerPrefs.HasKey("Tutorial")){
            if (PlayerPrefs.GetInt("Tutorial")==0){
                if(tutorial != null){
                    SetUINotActive(tutorial);
                }
                UIController.Instance.SetUIActive(tonOff);
                UIController.Instance.SetUIActive(toffOn);
            }else{
                UIController.Instance.SetUIActive(tonOn);
                UIController.Instance.SetUIActive(toffOff);
            }
        }else{
            PlayerPrefs.SetInt("Tutorial",1);
            UIController.Instance.SetUIActive(tonOn);
            UIController.Instance.SetUIActive(toffOff);
        }


        if (DataManager.Instance.needTutorial==1){
                if(blurCanvas != null && blurLayer!=null){
                    SetUIActive(blurCanvas);
                    SetUIActive(blurLayer);
                    //DataManager.Instance.SetNeedTutorial(1); 
                }
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


    public void DisableTutorial()
    {
        PlayerPrefs.SetInt("Tutorial",0);
        if(tutorial != null){
            SetUINotActive(tutorial);
        }
    }

    // Function to explicitly unmute the audio
    public void EnableTutorial()
    {
        PlayerPrefs.SetInt("Tutorial",1);
        if(tutorial != null){
            SetUIActive(tutorial);
        }
    }

}
