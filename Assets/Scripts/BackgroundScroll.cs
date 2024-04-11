using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float const_speed;
    public float speed; // control scroll speed from unity, for the camera itis -1, background is -0.8
    private Vector3 StartPos; // initial starting position
    public GameObject loopBG;
    public SpriteRenderer spriteRenderer; // get the sprite renderer of the background
    public PlayerMovement player;

    
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position; // get initial starting pos of background
        speed = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Continue();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation: Vector3.down * speed * Time.deltaTime); // move bg based on speed
        if (transform.position.y < player.transform.position.y-120 ){
            SpriteRenderer loopBGRender = loopBG.GetComponent<SpriteRenderer>(); // get the sprite renderer of the background
            Vector3 newPos = loopBGRender.transform.position;
            newPos.y = loopBGRender.bounds.max.y + spriteRenderer.bounds.size.y/2-10;
            transform.position = newPos;
        }
    }

    public void Stop(){
        speed = 0;
    }

    public void Continue(){
        speed = const_speed;
    }
}
