using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class LevelTimer : MonoBehaviour
{
    public float levelTime;
    public TextMeshProUGUI timerText;

    void Awake()
    {
        levelTime = 0f;
    }
    
    // Start is
    //  called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        levelTime += Time.deltaTime;
        timerText.text = "Time: " + levelTime.ToString("F2") + "s";
    }
}
