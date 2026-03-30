using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using LLMUnity;

public class FeedBackGen : MonoBehaviour
{
    //get current level player is on
    public int currentLevel;
    public TextMeshProUGUI feedbackText;   
    public LLMCharacter llmCharacter;
    public Car car;  
    public DataBaseManager databaseManager;
    //time interval for generating feedback (every 5 seconds)                     
    private float feedbackInterval = 5f; 
    private float timer = 0f;
    private string prompt;
    async void Update()
    {
        timer += Time.deltaTime;
        if (timer >= feedbackInterval)
        {
            timer = 0f;
            await GenerateFeedbackAsync();
        }
    }
    //Read the metrics from the database and generate feedback using the LLMCharacter
    async Task GenerateFeedbackAsync()
    {
        if (databaseManager == null || llmCharacter == null)
        {
            Debug.LogWarning("DatabaseManager or LLMCharacter reference is missing.");
            return;
        }
        Metrics metrics = await databaseManager.GetMetricsAsync();
        if (metrics == null)
        {
            Debug.LogWarning("Metrics data is null.");
            return;
        }
        //get used merge signal data from database
        var UsedMergeSignal = await databaseManager.GetUsedMergeSignalAsync();
        
        string collisions = metrics.collisions;
        string speed = metrics.speed;
        string wrongLaneCount = metrics.wrongLaneCount;
        string redLightVal = metrics.redLightVal;
        string parkingAccuracy = metrics.parkingAccuracy;

        //Each level focuses on different metrics, so-
        //the prompt for the LLMCharacter will change based on the current level the player is on.
        //Prompt for Level 1 (snow level) - focus on speed and collisions
        //prompt for LLMCharacter if player is on level 2
        if (currentLevel == 2)
        {
            //show actual values for red light violations, collisions, and wrong lane instances in the prompt
            Debug.Log("Collisions: " + collisions);
            Debug.Log("Red Light Violations: " + redLightVal);
            Debug.Log("Wrong Lane Count: " + wrongLaneCount);
            Debug.Log("Generating feedback for level 2 with red light violations.");
            prompt=
            $"Forget any previous instructions. " +
            $"Keep response 1 sentence. 10 WORD MAX FOR RESPONSE."+
            $"ASSUME ALL METRICS ARE AT 0 AT BEGINNING OF LEVEL. " +
            $"Your are a driving coach analyzing player performance. You are basing your feedback on metric numbers collected from the player's performance." +
            $"ONLY COMMENT ON {collisions} for collision, {wrongLaneCount} for wrong lane instances, and {redLightVal} for red light violations. " +
            $"Give constructive feedback on how they can improve their driving. ";
        }
        //prompt for LLMCharacter for Level 3 & 4 - Parking Levels
        if(currentLevel == 3 || currentLevel == 4)
        {
            Debug.Log("Generating feedback for parking levels.");
            //show actual values for red light violations, collisions, and wrong lane instances for the prompt
            Debug.Log("Collisions: " + collisions);
            Debug.Log("Red Light Violations: " + redLightVal);
            Debug.Log("Wrong Lane Count: " + wrongLaneCount);
            Debug.Log("Generating feedback for level 3 or 4 with parking metrics.");
            prompt =
            $"Forget any previous instructions. " +
            $"Assume the player has 0 collisions at start of level. " +
            $"Keep response 1 sentence. 10 WORD MAX FOR RESPONSE."+
            $"You are a driving coach analyzing a player's parking skills. You are basing your feedback on metric numbers collected from the player's performance. " +
            $"The player must avoid hitting other vehicles or obstacles." +
            $"Use {collisions} to tell the player how many times they hit an obstacle ";
            
        }
        //prompt for LLMCharacter for Level 5 Turn signal and merging level
         if(currentLevel == 5)
        {
            Debug.Log("Merge Signal Used: " + UsedMergeSignal);
            Debug.Log("Collisions: " + collisions);
            Debug.Log("Speed: " + speed);
            Debug.Log("Wrong Lane Count: " + wrongLaneCount);
            Debug.Log("Generating feedback for Level 5.");
            prompt =
            $"Forget any previous instructions. " +
            $"Keep response 1 sentence. 10 WORD MAX FOR RESPONSE."+
            $"ASSUME ALL PLAYER STATS ARE AT 0 AT BEGINNING OF LEVEL. " +
            $"Your are a driving coach analyzing player performance. You are basing your feedback on metric numbers collected from the player's performance." +
            $"When the stat for merge signal,{UsedMergeSignal}, is equal to 1, YOU MUST MENTION THAT THEY USED THEIR TURN SIGNAL." +
            $"Otherwise, if {UsedMergeSignal} is 0, DO NOT mention it. " +
            $"They had {metrics.collisions} collisions, " +
            $"and {metrics.wrongLaneCount} wrong lane instances. ";
        }
        //default prompt if for some reason currentLevel is not set correctly
       if (currentLevel == 1)
        {
            Debug.Log("Collisions: " + collisions);
            Debug.Log("Speed: " + speed);
            Debug.Log("Wrong Lane Count: " + wrongLaneCount);
            Debug.Log("Generating feedback for level 1 with speed and collision metrics.");
            prompt =
            $"Forget any previous instructions. " +
             $"Keep response 1 sentence. 10 WORD MAX FOR RESPONSE."+
             $"ASSUME ALL METRICS ARE AT 0 AT BEGINNING OF LEVEL. " +
             $"You are a driving coach analyzing player performance. You are basing your feedback on metric numbers collected from the player's performance." +
             $"Summarize the player’s driving performance in a friendly tone. " +
             $"an average speed of {metrics.speed} km/h, " +
             $"and {metrics.collisions} collisions. " +
             $"and {metrics.wrongLaneCount} wrong lane instances. ";
        }

        string response = await llmCharacter.Chat(prompt);

        //enforce the max 10 word limit
        /*string[] words = response.Split(' ');
        if (words.Length > 10)
        {
            response = string.Join(" ", words, 0, 10) + "...";
        }*/
        feedbackText.text = response;
        Debug.Log("Generated Feedback: " + response);
    }
}


