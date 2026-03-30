using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Button[] buttons;
    public DataBaseManager dbManager;
    private void Awake()
    {   //for testing purposes
        //PlayerPrefs.DeleteKey("UnlockedLevel");


        //Get actual index number of unlocked levels
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        //if current index is less than unlocked level, make button interactable
        for (int i =0; i < buttons.Length; i++)
        {
             buttons[i].interactable = false;
        }
        //else make interactable
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void Start()
    {
        UnlockedLevel();
    }
     //return the current unlocked level to DatabaseManager
    public void UnlockedLevel()
    {
        //if method is called
        Debug.Log("UnlockedLevel() CALLED");
        
        //get level index number
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        //save that level locally
        PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
        PlayerPrefs.Save();

        //save to database
       dbManager.SaveLevelProgress(unlockedLevel);
        
    }
    public void LoadLevel(string levelName)
    {
        //when level button is clicked, load that level
        SceneManager.LoadScene(levelName);
    }
    
}
