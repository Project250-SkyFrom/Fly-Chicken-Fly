using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public SpriteRenderer windSpriteRenderer; // Renderer for the wind animation sprite
    public List<Sprite> windSprites; // Wind animation sprite
    public float windForce = 0.1f; // Strength of the wind force, adjusted for continuous application
    public float windDuration = 3f; // Duration to apply wind force
    public float minWindInterval = 5f; // Minimum interval between wind forces
    public float maxWindInterval = 10f; // Maximum interval between wind forces
    public float animationFrameRate = 0.1f;
    public GameObject statusBar;
    public StatusBar charger;
    public GameObject windCanvas;

    private Rigidbody2D playerRigidbody;
    private float windDirection; // Track the wind direction globally within the script
    private float animationTimer;

    void Start()
    {
        playerRigidbody = player.GetComponent<Rigidbody2D>();
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

            AudioController.Instance.StartWindSound();

            windSpriteRenderer.flipX = windDirection < 0;

            // Show and animate wind sprites
            StartCoroutine(AnimateWind());

            statusBar.SetActive(true);
            windCanvas.SetActive(true);

            // Wait for the wind duration to pass
            yield return new WaitForSeconds(windDuration);

            charger.ResetFill();
            statusBar.SetActive(false);
            windCanvas.SetActive(false);

            // Hide wind animation sprite
            windSpriteRenderer.enabled = false;

            AudioController.Instance.StopWindSound();
        }
    }

    IEnumerator AnimateWind()
    {
        windSpriteRenderer.enabled = true;
        animationTimer = 0; // Reset animation timer
        int currentSpriteIndex = 0; // Start from the first sprite

        while (animationTimer < windDuration)
        {
            windSpriteRenderer.sprite = windSprites[currentSpriteIndex];
            yield return new WaitForSeconds(animationFrameRate);
            currentSpriteIndex = (currentSpriteIndex + 1) % windSprites.Count; // Cycle back to the first sprite after reaching the end
            animationTimer += animationFrameRate;
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
