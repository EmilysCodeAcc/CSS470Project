using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour
{
    public string nameofPlayer;
    public string saveName;
    public TMP_Text inputText;
    public TMP_Text loadName;

    // Update is called once per frame
    void Update()
    {
        nameofPlayer = PlayerPrefs.GetString("name", "none");
        loadName.text = "Player Name: " + nameofPlayer;
    }
    public void SaveName()
    {
        PlayerPrefs.DeleteKey("UnlockedLevel");
        saveName = inputText.text;
        PlayerPrefs.SetString("name", saveName);
    }
}
