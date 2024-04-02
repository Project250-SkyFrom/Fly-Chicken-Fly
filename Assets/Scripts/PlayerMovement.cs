using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    public GameObject windSpriteRenderer; // Reference to the wind sprite renderer

    public float speed;
    public float jump;
    public bool isJumping;
    public bool ableToMove;
    public bool withPiggyback;
    public float piggybackJump;
    public float jumpConstant;
    

    // Animation variables
    public float walkingFrameRate = 0.1f; // Adjust this value to control walking animation speed
    public float jumpingFrameRate = 0.1f; // Adjust this value to control jumping animation speed
    public float idleFrameRate = 0.1f; // Adjust this value to control idle animation speed
    public float hurtFrameRate = 0.1f; // Adjust this value to control hurt animation speed
    public float hurtDuration = 0.5f; // how long chicken is hurt for
    public float tiredWalkingFrameRate = 0.1f; // Adjust this value to control tired walking animation speed
    public float tiredJumpingFrameRate = 0.1f; // Adjust this value to control tired jumping animation speed
    public List<Sprite> animationFrames; // List to hold the walking animation sprites
    public List<Sprite> jumpAnimationFrames; // Holds jumping sprites
    public List<Sprite> idleAnimationFrames; // Holds idle sprites
    public List<Sprite> tiredWalkingFrames; // List to hold tired walking animation sprites
    public List<Sprite> tiredJumpingFrames; // Holds tired jumping sprites
    public Sprite hurtSprite; // Sprite for the hurt animation
    public List<Sprite> thunderHurtFrames; // List to hold thunder hurt animation sprites
    public float thunderHurtFrameRate = 0.1f; // Adjust this value to control thunder hurt animation speed
    public float thunderHurtDuration = 0.5f; // Duration of thunder hurt animation
    

    private int currentFrame = 0;
    private float frameTimer = 0f;
    private bool isIdle = false;
    private bool isHurt = false;
    private bool isHurtByThunder = false;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (withPiggyback)
        {
            jump = piggybackJump;

        }
        else
        {
            jump = jumpConstant;
        }
        if (ableToMove)
        {
            float move = 0f;

            // Check for W, A, D keys
            if (Input.GetKey(KeyCode.W))
            {
                // Handle jump logic
                if (!isJumping)
                {
                    body.velocity = new Vector2(body.velocity.x, jump);
                    isJumping = true;

                    AudioController.Instance.PlayChickenJump();

                    if (jumpAnimationFrames.Count > 0 && spriteRenderer != null)
                    {
                        spriteRenderer.sprite = jumpAnimationFrames[0];
                    }

                    AnimatePlayer(jumpingFrameRate);
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                // Handle left movement
                move = -1f;
                isIdle = false;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                // Handle right movement
                move = 1f;
                isIdle = false;
            }
            else
            {
                isIdle = true;
            }

            if (isIdle)
            {
                AnimatePlayer(idleFrameRate); // Use idle frame rate when player is idle
            }


            Vector2 newVelocity = new Vector2(move * speed, body.velocity.y);

            // Adjust player movement based on the current boundaries
            Vector2 newPosition = transform.position + new Vector3(newVelocity.x, 0, 0) * Time.deltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, GetMinXBoundary(), GetMaxXBoundary());

            // Apply the adjusted position directly or adjust velocity accordingly
            if (Time.deltaTime > 0)
            {
                body.velocity = new Vector2((newPosition.x - transform.position.x) / Time.deltaTime, body.velocity.y);
            }


            if (move < 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (move > 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }

            AnimatePlayer(walkingFrameRate);
        }
        windSpriteRenderer.transform.position = new Vector3(windSpriteRenderer.transform.position.x, spriteRenderer.transform.position.y, windSpriteRenderer.transform.position.z);
    }

    void AnimatePlayer(float currentFrameRate)
    {
        // Check if the spriteRenderer is not null
        if (spriteRenderer != null)
        {
            if (isHurt)
            {
                if (isHurtByThunder && thunderHurtFrames.Count > 0)
                {
                    frameTimer += Time.deltaTime;

                    if (frameTimer >= thunderHurtFrameRate)
                    {
                        frameTimer = 0f;
                        currentFrame = (currentFrame + 1) % thunderHurtFrames.Count;
                        spriteRenderer.sprite = thunderHurtFrames[currentFrame];
                    }
                }
                else if (hurtSprite != null)
                {
                    spriteRenderer.sprite = hurtSprite; // Display the regular hurt sprite
                }
            }
            else if (withPiggyback) // If the player is carrying a baby chicken
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) // Tired walking animation
                {
                    frameTimer += Time.deltaTime;

                    if (frameTimer >= tiredWalkingFrameRate)
                    {
                        frameTimer = 0f;
                        currentFrame = (currentFrame + 1) % tiredWalkingFrames.Count;
                        spriteRenderer.sprite = tiredWalkingFrames[currentFrame];
                    }
                }
                else if (Input.GetKey(KeyCode.W)) // Tired jumping animation
                {
                    frameTimer += Time.deltaTime;

                    if (frameTimer >= tiredJumpingFrameRate)
                    {
                        frameTimer = 0f;
                        currentFrame = (currentFrame + 1) % tiredJumpingFrames.Count;
                        spriteRenderer.sprite = tiredJumpingFrames[currentFrame];
                    }
                }
                else // Idle animation when not moving
                {
                    // Use idle frame rate when player is idle
                    frameTimer += Time.deltaTime;
                    if (frameTimer >= idleFrameRate)
                    {
                        frameTimer = 0f;
                        currentFrame = (currentFrame + 1) % idleAnimationFrames.Count;
                        spriteRenderer.sprite = idleAnimationFrames[currentFrame];
                    }
                }
            }
            else // Regular walking and jumping animations
            {
                // Check if any movement key is pressed and the player is not idle
                bool isMoving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !isIdle;

                if (!isJumping && !isMoving && idleAnimationFrames.Count > 0) // If the player is not jumping and not moving, play the idle animation
                {
                    frameTimer += Time.deltaTime;

                    if (frameTimer >= idleFrameRate)
                    {
                        frameTimer = 0f;
                        currentFrame = (currentFrame + 1) % idleAnimationFrames.Count;
                        spriteRenderer.sprite = idleAnimationFrames[currentFrame];
                    }
                }
                else if (isJumping && jumpAnimationFrames.Count > 0) // If the player is jumping, cycle through the jump animation frames
                {
                    frameTimer += Time.deltaTime;

                    if (frameTimer >= jumpingFrameRate)
                    {
                        frameTimer = 0f;
                        currentFrame = (currentFrame + 1) % jumpAnimationFrames.Count;
                        spriteRenderer.sprite = jumpAnimationFrames[currentFrame];
                    }
                }
                else if (isMoving && animationFrames.Count > 0) // If the player is moving, play the walking animation
                {
                    frameTimer += Time.deltaTime;

                    if (frameTimer >= currentFrameRate)
                    {
                        frameTimer = 0f;
                        currentFrame = (currentFrame + 1) % animationFrames.Count;
                        spriteRenderer.sprite = animationFrames[currentFrame];
                    }
                }
            }
        }
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("MovingPlatform") || other.gameObject.CompareTag("Spike"))
        {
            isJumping = false;
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            isHurt = true;
            isHurtByThunder = false; // Reset the thunder hurt flag

            StartCoroutine(EndHurtAnimation());
        }
        else if (other.gameObject.CompareTag("Thunder"))
        {
            isHurt = true;
            isHurtByThunder = true; // Set the thunder hurt flag

            StartCoroutine(EndHurtAnimation());
        }
    }


    float GetMinXBoundary()
    {
        return EventController.Instance.GetMinXBoundary();
    }

    float GetMaxXBoundary()
    {
        return EventController.Instance.GetMaxXBoundary();
    }

    IEnumerator EndHurtAnimation()
    {
        yield return new WaitForSeconds(thunderHurtDuration);
        isHurt = false;
        isHurtByThunder = false; // Reset the thunder hurt flag after the hurt animation ends
    }
}

