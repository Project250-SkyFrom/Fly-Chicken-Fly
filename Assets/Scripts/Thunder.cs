using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.gameObject.CompareTag("Player")){
            if (EventController.Instance.player.isInvincible){
                //add interaction sound here
            }else{
                EventController.Instance.AddLump();
                AudioController.Instance.PlayElectrocution();
                AudioController.Instance.PlayChickenHit();
            }
            Destroy(gameObject);
        }
    } 
}
