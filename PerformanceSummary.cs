using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using LLMUnity;

public class PerformanceSummary : MonoBehaviour
{
    [Header("References")]
    public LLMCharacter llmCharacter;          
    public DataBaseManager databaseManager;    
    public TextMeshProUGUI summaryText;       
    private bool summaryGenerated = false;
    string prompt;
    public int currentLevel;

    //Activate this function when the level ends to generate a performance summary for the player
    public async void OnLevelEnd()
    {
        if (summaryGenerated)
            return; 

        summaryGenerated = true;

        await GeneratePerformanceSummaryAsync();
    }

    //Read the metrics from the database and generate a performance summary using the LLMCharacter
    private async Task GeneratePerformanceSummaryAsync()
    {
        if (llmCharacter == null || databaseManager == null)
        {
            Debug.LogWarning("Missing references in PerformanceSummary.cs!");
            return;
        }
        // Get the player's performance metrics and light info from the database
        var metrics = await databaseManager.GetMetricsAsync();
        var UsedMergeSignal = await databaseManager.GetUsedMergeSignalAsync();
        //var lightInfo = await databaseManager.GetLightInfoAsync();
        if (metrics == null)
        {
            summaryText.text = "No performance data available.";
            return;
        }
        if(currentLevel == 1) //Snow Level - focus on speed and collisions
        {
            Debug.Log("Generating performance summary for level 1 with speed and collision metrics.");
            prompt =
             $"Write 2 short sentences." +
             $"You are a driving coach analyzing player performance" +
             $"Summarize the player’s driving performance in a friendly tone. " +
             $"ONLY TALK ABOUT THESE THINGS, DO NOT MENTION TURN SIGNAL: " +
             $"an average speed of {metrics.speed} km/h, " +
             $"and {metrics.collisions} collisions. " +
             $"and {metrics.wrongLaneCount} wrong lane instances. ";
        }
        if(currentLevel == 2)//Traffic Light Level - focus on red light violations
        {
            Debug.Log("Generating performance summary for level 2 with red light violation metrics.");
            prompt =
             $"Write 2 short sentences." +
             $"You are a driving coach analyzing player performance" +
             $"Summarize the player’s driving performance in a friendly tone. " +
             $"They had {metrics.redLightVal} red light violations. " +
             $"They had {metrics.collisions} collisions. " +
             $"and {metrics.wrongLaneCount} wrong lane instances. ";           
        }
        if (currentLevel == 3 || currentLevel == 4) //Parking Level - focus on parking accuracy
        {
            Debug.Log("Generating performance summary for level 3 with parking accuracy metrics.");
            prompt =
             $"Write 2 short sentences." +
             $"You are a driving coach analyzing player performance" +
             $"Summarize the player’s parking performance in a friendly tone. " +
             $"They had {metrics.parkingAccuracy} parking accuracy. " +
             $"They had {metrics.collisions} collisions. " +
             $"and {metrics.wrongLaneCount} wrong lane instances. ";           
        }
        if (currentLevel == 5) //Final Level - focus on all metrics
        {
            prompt =        
            $"Write 2 short sentences" +  
            $"You are a driving instructor AI. " +
            $"Summarize the player’s driving performance in a friendly tone. " +
            $"if {UsedMergeSignal} is over 1 you MUST mention that they used their turn signal." +
            $"They had {metrics.collisions} collisions, " +
            $"an average speed of {metrics.speed} km/h, " +
            $"and {metrics.wrongLaneCount} wrong lane instances. " +
            $"and {metrics.redLightVal} red light violations. " +
            $"if {metrics.parkingAccuracy} is 0.00, do not mention parking accuracy.";

        }
        
        string response = await llmCharacter.Chat(prompt);
        summaryText.text = response;
        Debug.Log("Generated Performance Summary: " + response);
    }
}