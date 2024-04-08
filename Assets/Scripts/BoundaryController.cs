using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    public float minPosX = -5f; // Min x position
    public float maxPosX = 5f;  // Max x position
    public float movingSpeed = 2f; // Speed of movement along x-axis
    private bool isMoving = false;
    public GameObject sharpEdge;
    public float oscillationSpeed = 5f; // Speed of the oscillation
    public float oscillationHeight = 2f; // Height of the oscillation

    public void Start()
    {
        // Start oscillation
        StartCoroutine(OscillateSharpEdge());
    }

    // Call this method to start moving
    public void MoveBoundary(bool toRight, float minPosX, float maxPosX)
    {
        this.minPosX = minPosX;
        this.maxPosX = maxPosX;
        if (!isMoving)
        {
            StartCoroutine(MoveBoundaryPosition(toRight));
        }
    }

    public void moveY(float y)
    {
        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    private IEnumerator MoveBoundaryPosition(bool toRight)
    {
        isMoving = true;
        float targetPosX = toRight ? maxPosX : minPosX;
        while (Mathf.Abs(transform.position.x - targetPosX) > 0.01f)
        {
            Vector3 newPos = transform.position;
            newPos.x = Mathf.MoveTowards(newPos.x, targetPosX, movingSpeed * Time.deltaTime);
            transform.position = newPos;
            yield return null;
        }
        isMoving = false;
    }

    private IEnumerator OscillateSharpEdge()
    {
        float initialRelativeY = sharpEdge.transform.position.y - transform.position.y;

        while (true) // Infinite loop to continuously oscillate and follow the wall
        {
            float newY = Mathf.Sin(Time.time * oscillationSpeed) * oscillationHeight + transform.position.y + initialRelativeY;
            
            sharpEdge.transform.position = new Vector3(sharpEdge.transform.position.x, newY, sharpEdge.transform.position.z);
            
            yield return null; // Wait for the next frame
        }
    }

}
