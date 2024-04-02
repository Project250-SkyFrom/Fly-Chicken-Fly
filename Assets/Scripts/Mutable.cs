using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mutable : MonoBehaviour
{
    private bool isMuted = false;
    // public GameObject sonOn;
    // public GameObject soffOff;
    // public GameObject sonOff;
    // public GameObject soffOn;

    void Awake(){
        // if (PlayerPrefs.HasKey("mute")){
        //     AudioListener.volume = PlayerPrefs.GetInt("mute");
        // }else{
        //     PlayerPrefs.SetInt("mute",1);
        //     AudioListener.volume = 1;
        // }
    }
    void Start(){
        // if (AudioListener.volume == 0){ //muted
        //     UIController.Instance.SetUIActive(sonOff);
        //     UIController.Instance.SetUIActive(soffOn);
        // }
        // else{
        //     UIController.Instance.SetUIActive(sonOn);
        //     UIController.Instance.SetUIActive(soffOff);
        // }
    }
    // Function to toggle mute state
    public void ToggleMute()
    {
        // isMuted = !isMuted;
        // AudioListener.volume = isMuted ? 0 : 1; // Mute or unmute all audio
    }

    // Function to explicitly mute the audio
    public void Mute()
    {
        // isMuted = true;
        // PlayerPrefs.SetInt("mute",0);
        // AudioListener.volume = 0; // Mute all audio
    }

    // Function to explicitly unmute the audio
    public void Unmute()
    {
        // isMuted = false;
        // PlayerPrefs.SetInt("mute",1);
        // AudioListener.volume = 1; // Unmute all audio
    }
}
