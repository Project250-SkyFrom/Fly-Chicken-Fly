using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    private static EventController _instance;
    public static EventController Instance { get { return _instance; } }

    private int lump=0;
    public int egg=0;

    public int shrinkScore = 30;
    public float minFieldOfView = 2f;
    public float shrinkAmount = 0.005f;
    public float initialBoundary = 0;

    public float minXBoundary = -1;
    public float maxXBoundary = 1;
    public bool startShrinking = false;

    public PlayerMovement player;
    public GameObject lump1;
    public GameObject lump2;
    public GameObject lump3;
    public GameObject x1;
    public GameObject x2;
    public GameObject x3;
    public GameObject piggyback;
    public float piggybackLiftTime;
    public float piggybackDestroyTime=0f;
    public string hardMode;
    public ObstacleGenerator obstacleGenerator;
    public EnemyMovement enemy;
    public LumpView lumpView;
    public AltitudeView altitudeView;
    public GoldenEggView eggView;
    public BackgroundScroll camera;
    public BackgroundScroll background;
    public BackgroundScroll BGTransDayNight;
    public BackgroundScroll BGTransNightDay;
    public Camera gameCamera;
    public GameObject gameOverUI;
    public BoundaryController leftBoundary;
    public BoundaryController rightBoundary;
    public float cameraGoDownDistance;
    public GameObject enemyObject;
    public bool isAblePowerUp;
    public int numOfPowerUp;

    void Awake(){
        _instance = this;
    }
    
    void Start(){
        ResumeGame();
    }
    void Update(){
       
        if (piggybackDestroyTime!=0f && Time.time > piggybackDestroyTime){
            DeleteBabyChicken();
        }

        if (initialBoundary.Equals(0)){
            UpdateXBoundary();
            initialBoundary = 1;
        }
        // Event when player touch the bottom of the camera, the camera should push the player up a jump
        if (player.transform.position.y < camera.transform.position.y - cameraGoDownDistance){
            Vector3 newPos = camera.transform.position;
            if (camera.transform.position.y - cameraGoDownDistance < 8.5f){
                newPos.y = 8.5f;
            }
            else{
                newPos.y -= 4; 
            }
            camera.transform.position = newPos; 
            // UpdateXBoundary();
        }
        // check bg to change gameMode
        if (player.transform.position.y > BGTransDayNight.transform.position.y){
            hardMode = "night";
            ChangeHardMode(hardMode);
        }
        else if (player.transform.position.y > BGTransNightDay.transform.position.y){
            hardMode = "day";
            ChangeHardMode(hardMode);
        }

        if (player.transform.position.y > shrinkScore){
            //Debug.Log("Y Position: " + player.transform.position.y + " Shrink Score: " + shrinkScore);
            if (!startShrinking){
                startShrinking = true;
                StartShrinkingBoundaries();
            }
        }
        leftBoundary.moveY(camera.transform.position.y);
        rightBoundary.moveY(camera.transform.position.y);
    }

    private void UpdateXBoundary(){
        float backgroundWidth = background.spriteRenderer.bounds.size.x; // Get the actual width of the background sprite
        float halfWidth = backgroundWidth / 2f;
        
        // Calculate boundaries
        minXBoundary = background.transform.position.x - halfWidth;
        maxXBoundary = background.transform.position.x + halfWidth;
    }

    public void AddLump(){
        lump += 1;
        if (lump == 1){
            lump1.SetActive(true);
            //x1.SetActive(true);
        }
        if (lump == 2){
            lump2.SetActive(true);
            //x2.SetActive(true);
        }
        if (lump == 3){
            lump3.SetActive(true);
            //x3.SetActive(true);
        }
        lumpView.UpdateLump(lump);
        if (DataManager.Instance.currentGameMode == "death" && lump >=3){
           //Debug.Log("End Game");
            EndGame();
        }
        //LumpGenerator.Instance.GenerateImages();
    }

    public void AddBabyChicken(){
        player.withPiggyback = true;
        piggyback.SetActive(true);
        piggybackDestroyTime = Time.time + piggybackLiftTime;
    }

    public void DeleteBabyChicken(){
        player.withPiggyback = false;
        piggyback.SetActive(false);
        piggybackDestroyTime = 0f;
    }

    public float GetMinXBoundary(){
        return minXBoundary;
    }

    public float GetMaxXBoundary(){
        return maxXBoundary;
    }

    public void StopCameraMoving(){
        camera.Stop();
    }

    public void StartCameraMoving(){
        camera.Continue();
    }

    public void StopBackgroundMoving(){
        background.Stop();
    }

    public void StartBackgroundMoving(){
        background.Continue();
    }


private void StartShrinkingBoundaries()
{   
    float newMaxPosX = Random.Range(10f, 15f); 
    float newMinPosX = Random.Range(14f, 22f);

    float control = Random.Range(0, 1);
    if (control > 0.5){
        rightBoundary.MoveBoundary(true, newMinPosX, newMaxPosX); 

        newMaxPosX = newMaxPosX + 7f;
        leftBoundary.MoveBoundary(false, -newMaxPosX, -newMinPosX); 
    } else {
        leftBoundary.MoveBoundary(false, -newMinPosX, -newMaxPosX); 
        newMaxPosX = newMaxPosX - 7f;
        rightBoundary.MoveBoundary(true, newMaxPosX, newMinPosX); 
    }

    startShrinking = false; 
}


    public void AddGoldenEgg(){
        egg+=1;
        eggView.UpdateGoldenEgg(egg);
        altitudeView.goldenEgg=egg;
        if (egg>=numOfPowerUp){
            isAblePowerUp = true;
        }
        //DataManager.Instance.UpdateGoldenEgg(egg);
    }

    public void LoseGoldenEgg(){
        if (egg>=5){
            egg -= 5;
        }
        else{
            egg = 0;
        }
        eggView.UpdateGoldenEgg(egg);
        altitudeView.goldenEgg=egg;
        if (egg<numOfPowerUp){
            isAblePowerUp = false;
        }
    }

    public void EndGame(){
        DataManager.Instance.UpdateGoldenEgg(egg);
        DataManager.Instance.UpdateHighestScore((int)altitudeView.score + egg*10,egg,(int)altitudeView.score);
        PauseGame();
        AudioController.Instance.stopBackgroundMusic();
        AudioController.Instance.PlayGameOverSounds();
        UIController.Instance.SetUIActive(gameOverUI);
        UIController.Instance.SetUINotActive(enemyObject);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Pauses the game
        player.ableToMove = false;
    }

    public void ResumeGame(){
        Time.timeScale = 1f; // Resume the game
        player.ableToMove = true;
    }
    
    public void ReplayGame()
    {
        Time.timeScale = 1f; // Reset time scale to normal in case it was paused
        SceneController.Instance.ChangeScene("PlayScene"); // Reload the current scene
    }

    public void ChangeHardMode(string mode){
        if (mode == "day"){
            obstacleGenerator.hardMode = "day";
        }
        else{   //night
            obstacleGenerator.hardMode = "night";
        }
    }
}
