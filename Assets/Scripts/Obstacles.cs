using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{

    public string type;
    public float fallingSpeed;
    public float horizontalSpeed;
    public float rotationSpeed;
    public int direction;
    public BackgroundScroll camera;
    public float disappear_distance;
    public SpriteRenderer movingPlatform;
    public float leftBound;
    public float rightBound;
    public float gapX;
    public bool eternal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (!eternal && transform.position.y<camera.transform.position.y-disappear_distance){
            Destroy(gameObject);
        }
        if (!eternal && type == "HandleUpKnife" && fallingSpeed == 0 && transform.position.y>camera.transform.position.y+0.4*disappear_distance){
                Destroy(gameObject);
        }
        
        else if (movingPlatform!=null){
            Vector3 newPos = transform.position;
            // if (movingPlatform.transform.position.x < 0){
            //     newPos.x = movingPlatform.transform.position.x - gapX;
            // }
            newPos.x = movingPlatform.transform.position.x + gapX;
            transform.position = newPos;
        }
        else if (!eternal){     //I have still objects for generation, so we don't want to chaneg any of them
            switch (type){
                case "polaCan":
                    fallingSpeed += 0.04f;
                    // Get the current rotation in Euler angles
                    Vector3 currentRotation = transform.rotation.eulerAngles;

                    // Update the Z rotation component
                    float newZRotation = currentRotation.z + rotationSpeed * Time.deltaTime;

                    // Ensure the rotation stays within 0 to 360 degrees
                    newZRotation = Mathf.Repeat(newZRotation, 360f);

                    // Construct new Euler angles with only the Z component changed
                    Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y, newZRotation);

                    // Set the rotation using Quaternion.Euler
                    transform.rotation = Quaternion.Euler(newRotation);

                    Vector3 newPos = transform.position;
                    newPos.y -= fallingSpeed*Time.deltaTime;
                    newPos.x += direction * horizontalSpeed*Time.deltaTime;
                    transform.position = newPos;
                    break;
                case "babyChicken":
                    fallingSpeed += 0.04f;
                    break;
                case "HandleUpKnife":
                    transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime +
                            Vector3.right * direction * horizontalSpeed * Time.deltaTime);
                    break;
                case "HandleDownKnife":
                    transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime +
                            Vector3.right * direction * horizontalSpeed * Time.deltaTime);
                    break;
            }
        }
        
        // transform.Translate(Vector3.down * fallingSpeed * Time.deltaTime +
        //                     Vector3.right * direction * horizontalSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D other){
        AudioSource audioSource = GetComponent<AudioSource>();
        switch (type)
        {
            case "HandleUpKnife":

                if (other.gameObject.CompareTag("Ground"))
                {
                    fallingSpeed = 0;
                    audioSource.Stop();
                }
                else if (other.gameObject.CompareTag("MovingPlatform") && movingPlatform==null)
                {
                    movingPlatform = other.gameObject.GetComponent<SpriteRenderer>();
                    // PlatformMovement movingInfo = other.gameObject.GetComponent<PlatformMovement>();
                    // horizontalSpeed = movingInfo.speed;
                    // leftBound = movingInfo.leftBound;
                    // rightBound = movingInfo.rightBound;
                    // direction = movingInfo.getDirection();
                    gapX = transform.position.x - movingPlatform.transform.position.x;
                    fallingSpeed = 0;
                    audioSource.Stop();
                }
                break;
            
            case "HandleDownKnife":
                // No action needed
                break;
            
            default:
                // None yet
                break;
        }

        if (other.gameObject.CompareTag("Player"))
        {
            if (EventController.Instance.player.isInvincible){
                //add interaction sound here
            }else{
                if (type == "babyChicken"){
                    Debug.Log("Hit bb");
                    EventController.Instance.AddBabyChicken();
                }
                else{
                    EventController.Instance.AddLump();
                }
                AudioController.Instance.PlayChickenHit();
            }
            Destroy(gameObject);
        }

    }

    void OnDestroy(){
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
