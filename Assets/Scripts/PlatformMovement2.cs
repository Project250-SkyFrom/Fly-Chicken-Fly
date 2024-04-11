using UnityEngine;

public class PlatformMovement2 : MonoBehaviour
{
    public float speed = 2f;
    public float upBound = 5f;   // Set the upper boundary for the platform
    public float downBound = -5f; // Set the lower boundary for the platform

    private int direction = 1;  // 1 for moving up, -1 for moving down

    void Update()
    {
        // Move the platform vertically
        transform.Translate(Vector3.up * direction * speed * Time.deltaTime);

        // Check if the platform is beyond the upper or lower boundary
        if (transform.position.y >= upBound || transform.position.y <= downBound)
        {
            // Change direction to move the platform in the opposite direction
            direction *= -1;
        }
    }
}



