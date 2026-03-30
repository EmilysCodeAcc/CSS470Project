using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CompletionCheck : MonoBehaviour
{
    public int levelIndex; //SnowLevel = 1, TrafficLights = 2, ect.

    public void OnLevelComplete()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        //unlock next level if not already unlocked
        if(levelIndex + 1 > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", levelIndex + 1);
           PlayerPrefs.Save();
        }
    }
}
