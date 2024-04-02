using UnityEngine;

public class VerticalPlatformMovement : MonoBehaviour
{
    public float speed = 2f;
    public float upBound = 3f;    // Set the upper boundary for the platform
    public float downBound = -3f; // Set the lower boundary for the platform

    private float startY;           // Initial y-position of the platform

    void Start()
    {
        // Store the initial y-position of the platform
        startY = transform.position.y;
    }

    void Update()
    {
        // Calculate the new y-position using PingPong function
        float newY = startY + Mathf.PingPong(Time.time * speed, Mathf.Abs(upBound - downBound)) + downBound;

        // Update the position of the platform
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}