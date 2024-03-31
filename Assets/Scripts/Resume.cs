using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public float elapsedTime;
    public float duration;
    public GameObject resume1;
    public GameObject resume2;
    public GameObject pauseButton;
    public bool resume_game;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;
        if (elapsedTime >= duration){
            elapsedTime = 0;
            UIController.Instance.SetUINotActive(resume1);
            if (resume_game){
                EventController.Instance.ResumeGame();
                UIController.Instance.SetUIActive(pauseButton);    
            }else{
                UIController.Instance.SetUIActive(resume2);
            }
        }
    }
}
