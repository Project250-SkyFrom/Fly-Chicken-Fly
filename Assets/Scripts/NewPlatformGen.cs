using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewPlatformGen : MonoBehaviour
{
    public GameObject regularPlatformPrefab;
    public GameObject movingPlatformPrefab;
    public List<GameObject> spikePlatformPrefab;
    public List<GameObject> rottenEggPlatformPrefab;
    public List<GameObject> thunderPlatformPrefab;
    public List<GameObject> babyChickenPlatformPrefab;
    public float minX = -7f;
    public float maxX = 6f;
    public float verticalDistanceBetweenPlatforms = 4.5f;
    public float timeBetweenPlatforms = 2f;
    public float startingYPosition = 5.77f;
    public int movingPlatformsFrequency = 3;
    public int spikePlatformFrequency = 5;
    public int rottenEggPlatformFrequency = 15;
    public int thunderPlatformFrequency = 9;
    public int babyChickenPlatformFrequency = 19;
    public float movingPlatformScale = 6.8f;
    public float spikePlatformScale = 6.8f;
    public float rottenEggPlatformScale = 6.8f;
    public float thunderPlatformScale = 6.8f;
    public float babyChickenPlatformScale = 6.8f;
    public bool isGenerating;
    public GameObject player;
    public float currentY = 5.77f;
    public int regularPlatformCounter = 0;
    public float generatingDistance;

    private List<GameObject> platforms = new List<GameObject>();

    void Start()
    {
        //StartCoroutine(GeneratePlatformsRepeatedly());
    }

    void Update()
    {
        

        if (isGenerating)
        {
            if (regularPlatformCounter == movingPlatformsFrequency)
            {
                GenerateMovingPlatform(currentY + movingPlatformScale);
                regularPlatformCounter++;
            }
            else if (regularPlatformCounter == spikePlatformFrequency)
            {
                GenerateSpikePlatform(currentY + spikePlatformScale);
                regularPlatformCounter++;
            }
            // check if it is on night mode and then generate thunder platform
            else if (EventController.Instance.hardMode == "night" && regularPlatformCounter == thunderPlatformFrequency)
            {
                Debug.Log("Thurder generated");
                GenerateThunderPlatform(currentY + thunderPlatformScale);
                regularPlatformCounter++;
            }
            else if (regularPlatformCounter == rottenEggPlatformFrequency)
            {
                GenerateRottenEggPlatform(currentY + rottenEggPlatformScale);
                regularPlatformCounter++;
            }
            else if (regularPlatformCounter == babyChickenPlatformFrequency)
            {
                GenerateBabyChickenPlatform(currentY + babyChickenPlatformScale);
                regularPlatformCounter = 0;
            }
            else
            {
                GenerateRegularPlatform(currentY);
                regularPlatformCounter++;
            }

            currentY += verticalDistanceBetweenPlatforms;
            isGenerating = false;
            //yield return new WaitForSeconds(0.1f);
            
        }
        if (currentY - player.transform.position.y < generatingDistance){        
            isGenerating = true;
            //yield return new WaitForSeconds(0.1f);
        }    
    }

    void GenerateRegularPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        GameObject platform = Instantiate(regularPlatformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
    }

    void GenerateMovingPlatform(float yPosition)
    {
        float randomX = Random.Range(minX, maxX);
        Vector2 spawnPosition = new Vector2(randomX, yPosition);
        GameObject platform = Instantiate(movingPlatformPrefab, spawnPosition, Quaternion.identity);
        platforms.Add(platform);
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
        GameObject thurdenPlatform = GetRandomPlatform(thunderPlatformPrefab);
        GameObject platform = Instantiate(thurdenPlatform, spawnPosition, Quaternion.identity);
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

    private GameObject GetRandomPlatform(List<GameObject> platformList)
    {
        // Generate a random index within the range of the list count
        int randomIndex = UnityEngine.Random.Range(0, platformList.Count);

        // Return the platform GameObject at the randomly generated index
        return platformList[randomIndex];
    }
}

