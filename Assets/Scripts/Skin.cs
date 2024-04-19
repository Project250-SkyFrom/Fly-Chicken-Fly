using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour
{
    // for any list of sprite, foloow the order:
    //  1-4: animationFrames
    //  5-10: jumpAnimationFrames
    //  11-15: idleAnimationFrames  the same as the animationFrame
    //  16 - 20: tiredWalkingFrames
    //  21-26: tiredJumpingFrames
    //  27: hurtSprite
    //  28-30 thunderHurtFrames
    //  31-38: eggCollisionFrames
    public PlayerMovement player;
    private List<Sprite> animationFrames;
    public SpriteRenderer playerRender;

    public List<Sprite> eggSkin;

    void Start()
    {   
        List<Sprite> animationFrames;
        if (PlayerPrefs.HasKey("skin")){
            switch (PlayerPrefs.GetString("skin")){
                case "eggSkin":
                animationFrames = eggSkin;
                break;
            }
        }
        for (int i =0; i<4; i++){
            player.animationFrames[i] = eggSkin[i];
        }
        for (int i=4;i<9;i++){
            player.jumpAnimationFrames[i-4] = eggSkin[i];
        }
        for (int i=10;i<14;i++){
            player.idleAnimationFrames[i-10] = eggSkin[i];
        }
        for (int i=15;i<19;i++){
            player.tiredWalkingFrames[i-15] = eggSkin[i];
        }
        for (int i=20;i<25;i++){
            player.tiredJumpingFrames[i-16] = eggSkin[i];
        }
        for (int i=26;i<27;i++){
            player.hurtSprite = eggSkin[i];
        }
        for (int i=28;i<30;i++){
            player.thunderHurtFrames[i-23] = eggSkin[i];
        }
        for (int i=31;i<38;i++){
            player.tiredJumpingFrames[i-16] = eggSkin[i];
        }
        //playerRender.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
