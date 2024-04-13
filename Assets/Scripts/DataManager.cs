using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public int numberOfEggs;
    public string playerName;
    public int numberOfGolds;
    public bool isLocked;
    private int maxLocalScore=5;
    public bool playCutScene = true;
    public List<int> highestScore = new List<int>(){0,0,0,0,0};  //the first one is the highest
    public List<int> goldenEggs = new List<int>(){0,0,0,0,0};
    public List<int> highestMeter = new List<int>(){0,0,0,0,0};
    public  List<string> playerNames = new List<string>(){"","","","",""};

    public int GetHighestScore(int index)
    {
        //Debug.Log(index);
        try{
            return highestScore[index];
        }
        catch{
            return highestScore[0];
        }
    }

    public int GetHighestMeter(int index)
    {
        //Debug.Log(index);
        try{
            return highestMeter[index];
        }
        catch{
            return highestMeter[0];
        }
    }

    public int GetGoldenEgg(int index)
    {
        //Debug.Log(index);
        try{
            return goldenEggs[index];
        }
        catch{
            return goldenEggs[0];
        }
    }

    public void SetHighestScore(int index, int highest,string playerName,int goldenEgg, int meter)
    {
        // Insert the new highest score at the specified position
        highestScore.Insert(index, highest);
        goldenEggs.Insert(index,goldenEgg);
        highestMeter.Insert(index,meter);
        playerNames.Insert(index,playerName);
        // If there are more than 5 scores in the list, remove the last one
        if (highestScore.Count > maxLocalScore)
        {
            highestScore.RemoveAt(5); // Removes the last element (index 5)
            goldenEggs.RemoveAt(5);
            highestMeter.RemoveAt(5);
            playerNames.RemoveAt(5); // Removes the last element (index 5)
        }
    }

    public string GetPlayerName(int index){
        return playerNames[index];
    }
    

    public int GetEggs()
    {
        return numberOfEggs;
    }

    public void SetEggs(int eggs)
    {
        numberOfEggs = eggs;
    }

    public int GetGold()
    {
        return numberOfGolds;
    }

    public void SetGold(int gold)
    {
        numberOfGolds = gold;
    }

    public bool IsLocked()
    {
        return isLocked;
    }

    public void SetLock(bool isLock)
    {
        isLocked = isLock;
    }

    public void SetPlayerName(string name){
        playerName = name;
    }
    public string GetPlayerName(){
        return playerName;
    }

    public void SetPlayCutScene(bool play){
        playCutScene = play;
    }

    public bool GetPlayCutScene(){
        return playCutScene;
    }
}


public class DataManager : MonoBehaviour
{
     //Singleton pattern
    private static DataManager _instance;
    public static DataManager Instance { get { return _instance; } }

    private string savedJson;
    private PlayerData loadedData;
    private int goldenEgg;
    public string currentGameMode;
    public string playerName=null;
    public bool playCutScene;
    public bool getNewHighestScore=false;
    private int maxLocalScore=5;
     public List<LeaderboardEntry> TopScores = new List<LeaderboardEntry>();

     void Awake(){      // I use awake here instead of Start because I need the highest score to be initialized for the Rankview to be shown
        _instance = this;

        DontDestroyOnLoad(gameObject); // Keep the EventController object persistent across scenes
        // Load the JSON string from PlayerPrefs
        savedJson = PlayerPrefs.GetString("playerData");
        loadedData = JsonUtility.FromJson<PlayerData>(savedJson) ?? new PlayerData();
        string updatedJson = JsonUtility.ToJson(loadedData);
        playCutScene = loadedData.GetPlayCutScene();
        // loadedData.SetPlayCutScene(true);
        // playCutScene=loadedData.GetPlayCutScene();
        // UpdateData();
        // FetchTopScores();
        // Debug.Log("Updated and Saved Player Data - Eggs: " + loadedData.GetEggs() +
        //       ", Golds: " + loadedData.GetGold() +
        //       ", Unlocked: " + loadedData.IsLocked() +
        //       ", Highest Score: " + loadedData.GetHighestScore(0));
        // for (int i = maxLocalScore-1; i>=0; i--){
        //      Debug.Log("Highest: " + loadedData.GetHighestScore(i));
        // }
    }

    void Start(){
        goldenEgg = loadedData.GetEggs();
        //Debug.Log(goldenEgg);

    }


    void UpdateData(){
        // Convert the modified data back to a JSON string
        string updatedJson = JsonUtility.ToJson(loadedData);

        // Save the updated JSON string to PlayerPrefs
        PlayerPrefs.SetString("playerData", updatedJson);
        PlayerPrefs.Save();
    }

    public void UpdateHighestScore(int current_height, int goldenEgg,int meter)
    {   
        //SubmitScoreToFirebase(current_height, playerName);
        int index = -1;
        for (int i = maxLocalScore-1; i>=0; i--){
            int previousHighest = loadedData.GetHighestScore(i);
            // Compare the current score with the previous high score
            if (current_height > previousHighest)
            {
                index = i;
            }
            else{
                break;
            }
        }
        // Update PlayerPrefs with the new high score
        if (index==0){
            loadedData.SetHighestScore(index,current_height,playerName,goldenEgg,meter);
            getNewHighestScore = true;
            // if (index == 0){
            //     RankingView.Instance.setScores(current_height);
            // }
            UpdateData();
        }
    }

    public void UpdateGoldenEgg(int egg){
        goldenEgg += egg;
        //Debug.Log(goldenEgg);
        loadedData.SetEggs(goldenEgg);
        UpdateData();
    }

    public void DontPlayVideo(){
        playCutScene = false;
        loadedData.SetPlayCutScene(playCutScene);
        UpdateData();
    }

    public void setZero(){      //reset the PlayerPrefs info
        
    }

    public int getHighestScore(int index){
        //return  highestScore;
        return loadedData.GetHighestScore(index); 
    }

    public int getGoldenEgg(int index){
        //return  highestScore;
        return loadedData.GetGoldenEgg(index); 
    }
    
    public int getHighestMeter(int index){
        //return  highestScore;
        return loadedData.GetHighestMeter(index); 
    }

    public void setPlayerName(string name){
        playerName = name;
        PlayerPrefs.SetString("plyaerName",playerName);
    }

    public string GetPlayerName(int index){
        return loadedData.GetPlayerName(index);
    }

    // public void SubmitScoreToFirebase(int score, string playerName){
    //     try {
    //         if (playerName == null || playerName == "") {
    //             playerName = "AnonymousPlayer#" + Random.Range(999, 100000).ToString();
    //         }
    //         var scoreEntry = new Dictionary<string, object>{
    //             { "playerName", playerName },
    //             { "score", score },
    //             { "timestamp", Firebase.Database.ServerValue.Timestamp }
    //         };
    //         Firebase.Database.DatabaseReference reference = Firebase.Database.FirebaseDatabase.DefaultInstance.GetReference("scores");
    //         reference.Push().SetValueAsync(scoreEntry);
    //     } catch {
    //         Debug.Log("Error submitting score to Firebase");
    //     }
    // }


    // public void FetchTopScores(){
    //     try{
    //     Firebase.Database.DatabaseReference reference = Firebase.Database.FirebaseDatabase.DefaultInstance.GetReference("scores");
    //     reference.OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWith(task =>
    //     {
    //         if (task.IsCompleted && !task.IsFaulted)
    //         {
    //             List<LeaderboardEntry> fetchedScores = new List<LeaderboardEntry>();
    //             foreach (var childSnapshot in task.Result.Children)
    //             {
    //                 string playerName = childSnapshot.Child("playerName").Value.ToString();
    //                 int score = int.Parse(childSnapshot.Child("score").Value.ToString());
    //                 fetchedScores.Add(new LeaderboardEntry(playerName, score));
    //             }

            
    //             fetchedScores.Reverse(); 
    //             UpdateTopScores(fetchedScores);
    //         }
    //     });
    //     } catch {
    //         Debug.Log("Error fetching top scores");
    //         }
    // }


    // public void UpdateTopScores(List<LeaderboardEntry> newTopScores)
    //     {
    //         TopScores = newTopScores;
    //     }

    // public int getGlobalHighestScore(int index){
    //     // Check if the index is within the bounds of the list
    //     if (index >= 0 && index < TopScores.Count)
    //     {
    //         return TopScores[index].Score;
    //     }
    //     else
    //     {
    //         // Return 0 if the index is out of range
    //         return 0;
    //     }
    // }

    // public string getGlobalPlayerName(int index) {
    //     // Check if the index is within the bounds of the list
    //     if (index >= 0 && index < TopScores.Count)
    //     {
    //         return TopScores[index].PlayerName;
    //     }
    //     else
    //     {
    //         // Return "Empty Spot" if the index is out of range
    //         return "Empty Spot";
    //     }
    // }
}

public class LeaderboardEntry
{
    public string PlayerName { get; set; }
    public int Score { get; set; }

    public LeaderboardEntry(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}
