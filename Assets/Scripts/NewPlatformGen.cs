using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPlatformGen : MonoBehaviour
{
    public GameObject regularPlatformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject nightMovingPlatformPrefab;
    public GameObject verticalMovingPlatformPrefab;
    public GameObject nightPlatformPrefab; // Define the night platform prefab
    public GameObject player;
    public GoldenEggGenerator dayGoldenEggGenerator;
    public GoldenEggGenerator dayMovingGoldenEggGenerator;
    public GoldenEggGenerator nightGoldenEggGenerator;
    public GoldenEggGenerator verticleGoldenEggGenerator;
    public List<GameObject> spikePlatformPrefab;
    public List<GameObject> rottenEggPlatformPrefab;
    public List<GameObject> thunderPlatformPrefab;
    public List<GameObject> babyChickenPlatformPrefab;
    private List<GameObject> platforms = new List<GameObject>();
    public ObstacleGenerator obsGen;


    public float minX = -7f;
    public float maxX = 6f;
    public float verticalDistanceBetweenPlatforms = 4.5f;
    public float timeBetweenPlatforms = 2f;
    public float startingYPosition = 5.77f;
    
    public int movingPlatformsFrequency = 3;
    public int nightMovingPlatformFrequency = 4;
    public int verticalMovingPlatformFrequency = 3;
    public int spikePlatformFrequency = 5;
    public int rottenEggPlatformFrequency = 15;
    public int thunderPlatformFrequency = 9;
    public int babyChickenPlatformFrequency = 19;
    public float movingPlatformScale = 6.8f;
    public float spikePlatformScale = 6.8f;
    public float rottenEggPlatformScale = 6.8f;
    public float thunderPlatformScale = 6.8f;
    public float babyChickenPlatformScale = 6.8f;
    public float verticalMovingPlatformScale = 6.8f;
    public float nightMovingPlatformScale = 10.7f;

    public bool isGenerating;
    public float currentY = 5.77f;
    public float generatingDistance;
    public int regularPlatformCounter = 0;

    public int dayGroupNum;
    public int dayMovingNum;
    public int nightGroupNum;
    public int verticleGroupNum;
    
    public string platformStage;
    public string nightPlatformStage;
    public bool allPlatform = false;  

    void Start()
    {
      
    }

    void Update()
    {   
        GetPlatformStage();
        if (isGenerating)
        {
            regularPlatformCounter = (regularPlatformCounter+1)%10;

            if (allPlatform){
                if (EventController.Instance.hardMode == "night")
                {
                    if (regularPlatformCounter == thunderPlatformFrequency )
                    {
                        Debug.Log("Thunder generated");
                        GenerateThunderPlatform(currentY + thunderPlatformScale);
                        
                    }

                    else if (regularPlatformCounter == nightMovingPlatformFrequency)

                    {
                        GenerateNightMovingPlatform(currentY + nightMovingPlatformScale);
                        regularPlatformCounter = 0;
                    }
                    else
                    {
                        GenerateNightPlatform(currentY);
                        //regularPlatformCounter = (regularPlatformCounter + 1) % 3;
                    }

                    currentY += verticalDistanceBetweenPlatforms;
                    isGenerating = false;
                }
                else
                {
                    if (regularPlatformCounter == movingPlatformsFrequency)
                    {
                        GenerateMovingPlatform(currentY + movingPlatformScale);

                    }
                    else if (regularPlatformCounter == spikePlatformFrequency)
                    {
                        GenerateSpikePlatform(currentY + spikePlatformScale);

                    }
                    else if (regularPlatformCounter == rottenEggPlatformFrequency)
                    {
                        GenerateRottenEggPlatform(currentY + rottenEggPlatformScale);

                    }
                    else if (regularPlatformCounter == verticalMovingPlatformFrequency)
                    {
                        GenerateVerticalMovingPlatform(currentY + verticalMovingPlatformScale);

                    }
                    else if (regularPlatformCounter == babyChickenPlatformFrequency)
                    {
                        GenerateBabyChickenPlatform(currentY + babyChickenPlatformScale);
                        regularPlatformCounter = 0;
                    }
                    else
                    {
                        GenerateRegularPlatform(currentY);

                    }

                    currentY += verticalDistanceBetweenPlatforms;
                    isGenerating = false;
                }

            }
            else{
                if (EventController.Instance.hardMode == "night"&& regularPlatformCounter%3==0)
                {
                    if (nightPlatformStage == "thunder" )
                    {
                        Debug.Log("Thunder generated");
                        GenerateThunderPlatform(currentY + thunderPlatformScale);
                        
                    }

                    else if (nightPlatformStage == "moving"&& regularPlatformCounter%3==0)

                    {
                        GenerateNightMovingPlatform(currentY + nightMovingPlatformScale);
                        regularPlatformCounter = 0;
                    }
                    else
                    {
                        GenerateNightPlatform(currentY);
                        //regularPlatformCounter = (regularPlatformCounter + 1) % 3;
                    }

                    currentY += verticalDistanceBetweenPlatforms;
                    isGenerating = false;
                }
                else
                {
                    if (platformStage == "moving"&& regularPlatformCounter%3==0)
                    {
                        GenerateMovingPlatform(currentY + movingPlatformScale);

                    }
                    else if (platformStage == "spike" && regularPlatformCounter%3==0 )
                    {
                        GenerateSpikePlatform(currentY + spikePlatformScale);

                    }
                    else if (platformStage == "rotten"&& regularPlatformCounter%3==0)
                    {
                        GenerateRottenEggPlatform(currentY + rottenEggPlatformScale);

                    }
                    else if (platformStage == "verticle"&& regularPlatformCounter%3==0)
                    {
                        GenerateVerticalMovingPlatform(currentY + verticalMovingPlatformScale);

                    }
                    else if (platformStage == "baby" && regularPlatformCounter%3==0)
                    {
                        GenerateBabyChickenPlatform(currentY + babyChickenPlatformScale);
                    }
                    else
                    {
                        GenerateRegularPlatform(currentY);

                    }

                    currentY += verticalDistanceBetweenPlatforms;
                    isGenerating = false;
                }

            }

        }

        if (currentY - player.transform.position.y < generatingDistance)
        {
            isGenerating = true;
        }
    }


    void GenerateRegularPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);

        // Check if it's night mode to decide which platform to generate
        GameObject platformToGenerate = EventController.Instance.hardMode == "night" ? nightPlatformPrefab : regularPlatformPrefab;

        int group = GenerateGoldenEgg(dayGroupNum,"day");
        GameObject platform = Instantiate(platformToGenerate, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
        RestoreGoldenEgg(group,"day");
    }

    void GenerateMovingPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        int group = GenerateGoldenEgg(dayMovingNum,"dayMoving");
        GameObject platform = Instantiate(movingPlatformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
        RestoreGoldenEgg(group,"dayMoving");
    }

    void GenerateVerticalMovingPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        int group = GenerateGoldenEgg(verticleGroupNum,"verticle");
        GameObject platform = Instantiate(verticalMovingPlatformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
        RestoreGoldenEgg(group,"verticle");
    }

    void GenerateSpikePlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        GameObject spikePlatform = GetRandomPlatform(spikePlatformPrefab);
        GameObject platform = Instantiate(spikePlatform, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
    }

    void GenerateRottenEggPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        GameObject rottenEggPlatform = GetRandomPlatform(rottenEggPlatformPrefab);
        GameObject platform = Instantiate(rottenEggPlatform, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
    }

    void GenerateThunderPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        GameObject thunderPlatform = GetRandomPlatform(thunderPlatformPrefab);
        GameObject platform = Instantiate(thunderPlatform, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
    }

    void GenerateBabyChickenPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        GameObject babyChickenPlatform = GetRandomPlatform(babyChickenPlatformPrefab);
        GameObject platform = Instantiate(babyChickenPlatform, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
    }

    void GenerateNightPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        int group = GenerateGoldenEgg(nightGroupNum,"night");
        GameObject platform = Instantiate(nightPlatformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
        RestoreGoldenEgg(group,"night");
    }

    void GenerateNightMovingPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        int group = GenerateGoldenEgg(nightGroupNum, "night");
        GameObject nightMovingPlatform = Instantiate(nightMovingPlatformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(nightMovingPlatform);
        RestoreGoldenEgg(group, "night");
    }

    private GameObject GetRandomPlatform(List<GameObject> platformList)
    {
        // Generate a random index within the range of the list count
        int randomIndex = UnityEngine.Random.Range(0, platformList.Count);

        // Return the platform GameObject at the randomly generated index
        return platformList[randomIndex];
    }

    private int GenerateGoldenEgg(int num, string type){
        int group = UnityEngine.Random.Range(1, num+1);
        switch(type){
            case "day":
                dayGoldenEggGenerator.SetGoldenEgg(group,true);
                break;
            case "dayMoving":
                dayMovingGoldenEggGenerator.SetGoldenEgg(group,true);
                break;
            case "night":
                nightGoldenEggGenerator.SetGoldenEgg(group,true);
                break;
            case "verticle":
                verticleGoldenEggGenerator.SetGoldenEgg(group,true);
                break;
        }
        return group;
    }

    private void RestoreGoldenEgg(int group,string type){
        switch(type){
            case "day":
                dayGoldenEggGenerator.SetGoldenEgg(group,false);
                break;
            case "dayMoving":
                dayMovingGoldenEggGenerator.SetGoldenEgg(group,false);
                break;
            case "night":
                nightGoldenEggGenerator.SetGoldenEgg(group,false);
                break;
            case "verticle":
                verticleGoldenEggGenerator.SetGoldenEgg(group,false);
                break;
        }
    }

    private void GetPlatformStage(){
        if (obsGen.difficultyLevel >= 5*3){
            allPlatform = true;
        }
        else if(obsGen.difficultyLevel >= 4*3){
            platformStage = "baby";
        }else if (obsGen.difficultyLevel >= 3*3){
            platformStage = "rotten";
        }else if (obsGen.difficultyLevel >= 2*3){
            nightPlatformStage = "moving";
            platformStage = "verticle";
        }else if (obsGen.difficultyLevel >= 1*3){
            platformStage = "moving";
        }else{
            platformStage = "spike";
            nightPlatformStage = "thunder";
        }
    }
}

