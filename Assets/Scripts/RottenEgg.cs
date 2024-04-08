using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RottenEgg : MonoBehaviour
{
    public List<Sprite> animationFrames;  // List of sprites for animation frames
    public float animationSpeed = 0.2f;   // Speed of the animation cycle
    public GameObject negativePointsPrefab; // Reference to the "-5" sprite prefab
    public float negativePointsDuration = 2f; // Duration the "-5" sprite is displayed
    private SpriteRenderer spriteRenderer;
    private int currentFrameIndex = 0;
    private float frameTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (animationFrames.Count == 0)
        {
            Debug.LogError("Animation frames are not properly set!");
            return;
        }
        // Set initial sprite
        spriteRenderer.sprite = animationFrames[currentFrameIndex];
    }

    // Update is called once per frame
    void Update()
    {
        // Update frame timer
        frameTimer += Time.deltaTime;

        // If it's time to change frame
        if (frameTimer >= animationSpeed)
        {
            // Move to the next frame
            currentFrameIndex = (currentFrameIndex + 1) % animationFrames.Count;
            // Update sprite
            spriteRenderer.sprite = animationFrames[currentFrameIndex];
            // Reset frame timer
            frameTimer = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventController.Instance.LoseGoldenEgg();
            AudioController.Instance.PlayRottenEgg();
            AudioController.Instance.PlayChickenHit();

            // Get the position of the rotten egg
            Vector3 eggPosition = transform.position;

            // Adjust the position slightly above the rotten egg
            Vector3 pointsPosition = eggPosition + new Vector3(-1f, 1f, 0f); // Adjust the y-coordinate as needed

            // Instantiate the "-5" sprite at the adjusted position
            GameObject negativePointsObject = Instantiate(negativePointsPrefab, pointsPosition, Quaternion.identity);

            // Destroy the "-5" sprite after the specified duration
            Destroy(negativePointsObject, negativePointsDuration);

            // Destroy the rotten egg
            //Destroy(gameObject);
        }
    }
}

