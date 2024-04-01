﻿using System.Collections;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public SpriteRenderer windSpriteRenderer; // Renderer for the wind animation sprite
    public Sprite windSprite; // Wind animation sprite
    public float windForce = 0.1f; // Strength of the wind force, adjusted for continuous application
    public float windDuration = 3f; // Duration to apply wind force
    public float minWindInterval = 5f; // Minimum interval between wind forces
    public float maxWindInterval = 10f; // Maximum interval between wind forces

    private Rigidbody2D playerRigidbody;
    private float windDirection; // Track the wind direction globally within the script

    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        windSpriteRenderer.sprite = windSprite;
        windSpriteRenderer.enabled = false; // Hide the sprite initially
        StartCoroutine(WindRoutine());
    }

    IEnumerator WindRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWindInterval, maxWindInterval));

            // Determine wind direction
            windDirection = Random.value > 0.5f ? 1f : -1f; // Randomly choose wind direction
            if (windDirection < 0)
            {
                transform.position = new Vector3(10f, transform.position.y, transform.position.z); // Flip the wind sprite
            }
            else
            {
                transform.position = new Vector3(-10f, transform.position.y, transform.position.z); // Flip the wind sprite
            }
            // Show wind animation sprite based on direction
            windSpriteRenderer.flipX = windDirection < 0;
            windSpriteRenderer.enabled = true;

            // Start applying wind force
            StartCoroutine(ApplyWindForce());

            // Wait for the wind duration to pass
            yield return new WaitForSeconds(windDuration);

            // Hide wind animation sprite
            windSpriteRenderer.enabled = false;
        }
    }

    IEnumerator ApplyWindForce()
    {
        float timer = 0;
        while (timer < windDuration)
        {
            playerRigidbody.AddForce(new Vector2(windForce * windDirection, 0), ForceMode2D.Force);
            timer += Time.deltaTime;
            yield return null; // Wait until next frame to continue
        }
    }
}
