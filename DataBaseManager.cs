using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using TMPro;
using System.Threading.Tasks;

public class DataBaseManager : MonoBehaviour
{
    public TMP_InputField Name;
    public TMP_Text Collisions;
    public TMP_Text speedText;
    public TMP_Text WrongLaneCount;
    
    private string userID;
    private string collisions;

    private DatabaseReference dbreference;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        // Get the root reference location of the database.
        dbreference = FirebaseDatabase.DefaultInstance.RootReference;

        //check if user exists, if so log message
        if(PlayerPrefs.GetInt("UserCreated", 0) == 1)
        {
            Debug.Log("User already exists");
        }
    }
    
   
    public void CreateUser()
    {
        //check if user already exists
       /* if(PlayerPrefs.GetInt("UserCreated", 0) == 1)
        {
            Debug.Log("User already exists");
            return;
        }*/
        
        User newUser = new User(Name.text);
        string json = JsonUtility.ToJson(newUser);
        dbreference.Child("users").Child(userID).SetRawJsonValueAsync(json);

        //save username locally
        PlayerPrefs.SetString("UserName", Name.text);
       PlayerPrefs.SetInt("UserCreated", 1);
       PlayerPrefs.Save();
    }
    public void UpdateMetrics(string collisions, string speed, string wrongLaneCount, string redLightVal, string parkingAccuracy)
    {
        if (dbreference == null)
        {
            Debug.LogWarning("Database reference is null — Firebase not ready yet.");
            return;
        }
        Metrics newMetrics = new Metrics(collisions, speed, wrongLaneCount, redLightVal, parkingAccuracy);
        string json = JsonUtility.ToJson(newMetrics);
        dbreference.Child("users").Child(userID).Child("metrics").SetRawJsonValueAsync(json);
    }
    //Store light info in their own section of database
    public void UpdateLightInfo(bool lightsOn, bool leftTurnSignalsOn, bool rightTurnSignalsOn)
    {
        if (dbreference == null)
        {
            Debug.LogWarning("Database reference is null — Firebase not ready yet.");
            return;
        }
        LightStateContructor newLightInfo = new LightStateContructor(lightsOn, leftTurnSignalsOn, rightTurnSignalsOn);
        string json = JsonUtility.ToJson(newLightInfo);
        dbreference.Child("users").Child(userID).Child("Light Info").SetRawJsonValueAsync(json);
    }
    public async Task<Metrics> GetMetricsAsync() 
{
    //find the metrics for the current user
    var dataSnapshot = await dbreference
        .Child("users")
        .Child(userID)
        .Child("metrics")
        .GetValueAsync();

    if (dataSnapshot.Exists)
    {   // Convert the JSON data to a Metrics object
        string json = dataSnapshot.GetRawJsonValue();
        Metrics metrics = JsonUtility.FromJson<Metrics>(json);
        return metrics;
    }
    else
    {
        Debug.LogWarning("No metrics found for user.");
        return null;
    }
}

//save level progress to database
public void SaveLevelProgress(int CurrentLevel)
    {
        dbreference.Child("users").Child(userID).Child("levelProgress").SetValueAsync(CurrentLevel);
    }

//Make usedMergeSignal savable
    public void UsedMergeSignal(int usedMergeSignal)
    {
        dbreference.Child("users").Child(userID).Child("usedMergeSignal").SetValueAsync(usedMergeSignal);
    }
    //Make light info retrievable
    public async Task<LightStateContructor> GetLightInfoAsync()
    {
        var dataSnapshot = await dbreference
            .Child("users")
            .Child(userID)
            .Child("Light Info")
            .GetValueAsync();

        if (dataSnapshot.Exists)
        {
            string json = dataSnapshot.GetRawJsonValue();
            LightStateContructor lightInfo = JsonUtility.FromJson<LightStateContructor>(json);
            return lightInfo;
        }
        else
        {
            Debug.LogWarning("No light info found for user.");
            return null;
        }
    }
    //Make usedMergeSignal retrievable
    public async Task<int> GetUsedMergeSignalAsync()
    {
        var dataSnapshot = await dbreference
            .Child("users")
            .Child(userID)
            .Child("usedMergeSignal")
            .GetValueAsync();

        if (dataSnapshot.Exists)
        {
            int usedMergeSignal = int.Parse(dataSnapshot.Value.ToString());
            return usedMergeSignal;
        }
        else
        {
            Debug.LogWarning("No UsedMergeSignal found for user.");
            return 0;
        }
    }
    
}
