using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // public GameObject bonOn;
    // public GameObject boffOff;
    // public GameObject bonOff;
    // public GameObject boffOn;
    public GameObject BGmusic;
    public GameObject tonOn;
    public GameObject toffOff;
    public GameObject tonOff;
    public GameObject toffOn;
    public GameObject tutorial;


    //Singleton pattern
    private static UIController _instance;
    public static UIController Instance { get { return _instance; } }


    void Awake(){
        _instance = this;
    }

    void Start(){
        // if (PlayerPrefs.HasKey("BGMute")){
        //     if (PlayerPrefs.GetInt("BGMute")==0){
        //         if(BGmusic != null){
        //             SetUINotActive(BGmusic);
        //         }
        //         UIController.Instance.SetUIActive(bonOff);
        //         UIController.Instance.SetUIActive(boffOn);
        //     }else{
        //         UIController.Instance.SetUIActive(bonOn);
        //         UIController.Instance.SetUIActive(boffOff);
        //     }
        // }else{
        //     PlayerPrefs.SetInt("BGMute",1);
        //     UIController.Instance.SetUIActive(bonOn);
        //     UIController.Instance.SetUIActive(boffOff);
        // }

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
    }

    public void SetUIActive(GameObject ui)
    {   
        ui.SetActive(true);
    }

    public void SetUINotActive(GameObject ui)
    {   
        ui.SetActive(false);
    }

    // public void MuteBG()
    // {
    //     PlayerPrefs.SetInt("BGMute",0);
    //     if(BGmusic != null){
    //         SetUINotActive(BGmusic);
    //     }
    // }

    // // Function to explicitly unmute the audio
    // public void UnmuteBG()
    // {
    //     PlayerPrefs.SetInt("BGMute",1);
    //     if(BGmusic != null){
    //         SetUIActive(BGmusic);
    //     }
    // }

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
