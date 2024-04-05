using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenEgg : MonoBehaviour
{
    public BackgroundScroll camera;
    public float disappear_distance;
    public bool eternal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!eternal && transform.position.y<camera.transform.position.y-disappear_distance){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Gain Egg");
            EventController.Instance.AddGoldenEgg();
            Destroy(gameObject);
            AudioController.Instance.PlayGoldenEggPick();
        }
        // else{   // for any overlapping
        //     Destroy(gameObject);
        // }
    }
}
