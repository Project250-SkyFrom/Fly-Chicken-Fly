using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GIFController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float frameTimer = 0f;
    public float frameRate;
    private int currentFrame=0;
    public List<Sprite> frames;

    void Start()
    {
        //animator.speed = slowdownFactor;
    }

    // Update is called once per frame
    void Update()
    {
        frameTimer += Time.deltaTime;

        if (frameTimer >= frameRate)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % frames.Count;
            spriteRenderer.sprite = frames[currentFrame];
        }
        
    }
}
