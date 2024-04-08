﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomWallController : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public GameObject spikeEdge; // Reference to the spike wall
    private Rigidbody2D body;
    public float moveSpeed = 3f; // Speed at which the spike wall moves
    public float maxDistance = 10f; // Maximum distance below the player
    public float catchUpDelay = 1f; // Time before spike wall starts moving when player is idle
    private float catchUpTimer;
    public float oscillationMagnitude = 2f; // How far the spike wall oscillates along the X axis
    public float oscillationSpeed = 5f; // How fast the spike wall oscillates
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        catchUpTimer = catchUpDelay;
        StartCoroutine(OscillateSpikeEdge());
    }

    void Update()
    {
        float playerYPos = player.position.y;
        float spikeWallYPos = transform.position.y;
        
        // Check if the player is far enough above the spike wall to trigger movement
        if (playerYPos - spikeWallYPos > maxDistance)
        {
            // Move the spike wall upwards at moveSpeed
            body.velocity = new Vector2(0, moveSpeed);
            catchUpTimer = catchUpDelay; // Reset the catch-up timer
        }
        else
        {
            // If the player is not far enough above, start or continue the countdown
            catchUpTimer -= Time.deltaTime;
            
            if (catchUpTimer <= 0)
            {
                // If the countdown has finished, move the spike wall upwards
                body.velocity = new Vector2(0, moveSpeed * 0.5f); // Move slower than the normal moveSpeed
            }
            else
            {
                // If the countdown is still going, stop moving
                body.velocity = new Vector2(0, 0);
            }
        }
    }
    
    private IEnumerator OscillateSpikeEdge()
    {
        while (true) // Infinite loop to keep the oscillation going
        {
            float oscillation = Mathf.Sin(Time.time * oscillationSpeed) * oscillationMagnitude + transform.position.x;
            spikeEdge.transform.position = new Vector3(oscillation, spikeEdge.transform.position.y, spikeEdge.transform.position.z);
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("End Game");
            if(DataManager.Instance.currentGameMode == "death"){
                EventController.Instance.EndGame();
                Destroy(gameObject);
            }
            
        }
    }

}
