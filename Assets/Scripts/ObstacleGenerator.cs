using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleGenerator : MonoBehaviour
{
    public GameObject handleUpKnifePrefab; // Prefab of the knife object
    public GameObject handleDownKnifePrefab;
    public GameObject polaCanPrefab;
    public GameObject babyChickenPrefab;
    public float generateRate; // Rate at which knives will be generated
    public float baseGenerationRate;
    public float dayGenerationRate;
    public float nightGenerationRate;
    public string hardMode;
    public float increaseDifficultyDistance;
    public int difficultyLevel;
    public float decreaseRate;
    private float nextGenerateTime; // Time when the next knife should be generated
    public BackgroundScroll camera;
    //private string[] obstaclesType = {"HandleUpKnife","HandleDownKnife","PolaCan","BabyChicken"};
    public string[] obstaclesType = {"HandleDownKnife"};
    private GameObject thisObstacle;
    public float handleUpKnifeGenerateHeight;
    public float polaCanStartGenerateHeight;

    public GameObject warningSignPrefab;
    public GameObject mainCamera;

    private float cameraWidth = 40.0f;  //should be update after the implementation of shrink and expand of camera
    void Start()
    {
        // Initialize nextGenerateTime to the current time plus the generateRate
        generateRate = baseGenerationRate+dayGenerationRate;
        nextGenerateTime = Time.time + generateRate;
    }

    void Update()
    {   
        if (Time.time >= nextGenerateTime - 2) // 2 seconds warning
        {
            StartCoroutine(ShowWarningSpriteCoroutine());
        }

        if (Time.time >= nextGenerateTime)
            {
                GenerateObstacle();
                nextGenerateTime = Time.time + generateRate;
            }
        
    }

    IEnumerator ShowWarningSpriteCoroutine()
    {
        Vector3 warningPosition = CalculateWarningPosition();
        GameObject warningSprite = Instantiate(warningSignPrefab, warningPosition, Quaternion.identity);
        
        yield return new WaitForSeconds(1f); // Keep the sprite for 1 second

        Destroy(warningSprite); // Destroy the sprite after 1 second
    }

    Vector3 CalculateWarningPosition()
    {
        Vector3 position = camera.transform.position;
        Vector3 cameraPosition = mainCamera.transform.position;

        position.y = cameraPosition.y + 10f;
        position.x = cameraPosition.x - 5f;

        return position;
    }

    void GenerateObstacle() {
        string type = GetRandomObstacleType();
        int direction = GetRandomDirection();
        if (hardMode == "night"){
            generateRate = baseGenerationRate+nightGenerationRate;
        }else{
            generateRate = baseGenerationRate+dayGenerationRate;
        }
        if (transform.position.y>=difficultyLevel*increaseDifficultyDistance){
            difficultyLevel+=1;
            baseGenerationRate-=decreaseRate;
        }
        switch(type){   // find that type of obstacle should be genrated
            case "HandleUpKnife":
                thisObstacle = handleUpKnifePrefab;
                break;
            case "HandleDownKnife":
                thisObstacle = handleDownKnifePrefab;
                break;
            case "PolaCan":
                thisObstacle = polaCanPrefab;
                break;
            case "BabyChicken":
                thisObstacle = babyChickenPrefab;
                break;
        }
        
        // Check if it's time to generate a knife
        if (Time.time >= nextGenerateTime)
        {
            Vector3 newPosition = camera.transform.position;
            newPosition.y = camera.transform.position.y + cameraWidth/2;
            switch (type){
                case "PolaCan":
                    if (direction == 1){
                        newPosition.x = camera.transform.position.x - cameraWidth/2;
                    }
                    else{
                        newPosition.x = camera.transform.position.x + cameraWidth/2;
                    }
                    break;
                case "BabyChicken":
                    if (direction == 1){
                        newPosition.x = camera.transform.position.x - cameraWidth/2;
                    }
                    else{
                        newPosition.x = camera.transform.position.x + cameraWidth/2;
                    }
                    break;
                default:
                    float randomOffset = Random.Range(-10f, 10f); // Random offset
                    newPosition.x = camera.transform.position.x + randomOffset; // New position with random offset
                    break;
            }
            transform.position = newPosition;
            // Instantiate a new knife object at the calculated position
            GameObject newObject = Instantiate(thisObstacle, newPosition, Quaternion.identity);
            // Play the obstacle falling sound once the obstacle is generated
            AudioController.Instance.PlayObstacleFall(newObject.GetComponent<AudioSource>());
            // change the newKnife z value to 0
            Vector3 newPos = newObject.transform.position;
            newPos.z = 0; 
            newObject.transform.position = newPos; 

            Obstacles newObstacle = newObject.GetComponent<Obstacles>();
            
            switch(type){
                case "PolaCan":
                    newObstacle.direction = direction;
                    newObstacle.horizontalSpeed = 10;
                    newObstacle.fallingSpeed =1;
                    newObstacle.rotationSpeed = 180f;
                    break;
                case "BabyChicken":
                    newObstacle.direction = direction;
                    newObstacle.horizontalSpeed = 10;
                    newObstacle.fallingSpeed =1;
                    break;
                default:
                    newObstacle.fallingSpeed =10;
                    break;
            }
            newObstacle.eternal = false;
            // Update nextGenerateTime for the next knife
            nextGenerateTime = Time.time + generateRate;
        }
    }

    private string GetRandomObstacleType()
    { 
        if(transform.position.y>=polaCanStartGenerateHeight){
            obstaclesType = new string[] {"HandleUpKnife","HandleDownKnife","PolaCan"};
        } 
        else if (transform.position.y>=handleUpKnifeGenerateHeight){
            obstaclesType = new string[] {"HandleUpKnife","HandleDownKnife"};
        }
        // Generate a random index within the range of the array length
        int randomIndex = UnityEngine.Random.Range(0, obstaclesType.Length);

        // Return the string at the randomly generated index
        return obstaclesType[randomIndex];
    }

    private int GetRandomDirection(){
        int randomIndex = UnityEngine.Random.Range(0, 2) * 2 - 1; // only return 1 or -1
        return randomIndex;
    }

}
