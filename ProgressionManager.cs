using UnityEngine;
using System;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;
    public int totalLevels = 4;
    public LevelProgress levelProgress;
    private const string LevelProgressKey = "LevelProgressData";
    public DataBaseManager dbManager;

   /* void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }   
     async void Start()
    {
       if(dbManager != null)
       {
            LevelProgress loadedProgress = await dbManager.LoadLevelProgressAsync(totalLevels);
            
            levelProgress = loadedProgress;
            SaveProgress(); // Save to PlayerPrefs after loading from database
       }
       
    }
    public void MarkCompleted(int levelIndex)
    {
        if(levelIndex < 0 || levelIndex >= totalLevels)
        {
            Debug.LogError("Invalid level index");
            return;
        }
        levelProgress.levelsCompleted[levelIndex] = true;  
        SaveProgress();
        if (dbManager != null)
    {
        dbManager.SaveLevelProgress(levelProgress);
    }
    else
    {
        Debug.LogWarning("DB Manager missing — progress not saved to Firebase.");
    }
    }
    public bool IsLevelUnlocked(int levelIndex)
    {
        if(levelIndex==0)
        {
            return true; // First level is always unlocked
        }
        return levelProgress.levelsCompleted[levelIndex - 1];
    }
    void SaveProgress()
    {
        string json = JsonUtility.ToJson(levelProgress);
        PlayerPrefs.SetString(LevelProgressKey, json);
        PlayerPrefs.Save();
    }
    void LoadProgress()
    {
        if (PlayerPrefs.HasKey(LevelProgressKey))
        {
            string json = PlayerPrefs.GetString(LevelProgressKey);
            levelProgress = JsonUtility.FromJson<LevelProgress>(json);
        }
        else
        {
            levelProgress = new LevelProgress(totalLevels);
        }
    }
    */
}
