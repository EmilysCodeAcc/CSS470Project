using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Script that controls checking for if user turns on their left side indicator
//Also turns on UI indicator for left signal
public class CheckSignal : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject leftSignalUI;
    public LightsScript lightsScript;
    public Car carScript;
    public DataBaseManager dbManager;
    public int leftSignalUsed = 0;
    void Start(){
        if (leftSignalUI != null)
            leftSignalUI.SetActive(false);   
    }
    void OnTriggerEnter(Collider other)
    {
        dbManager.UsedMergeSignal(leftSignalUsed);  
        if (other.CompareTag(playerTag))
        {
            carScript.canMove = false;
            if (leftSignalUI != null)
            {
                leftSignalUI.SetActive(true);    
                Invoke("HideLeftSignalUI", 5f); 
            }
        }
    }
    //while in the trigger
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            //if left signal is off, stop player from proceeding
            if(!lightsScript.leftTurnSignalOn)
            {
                //only show debug message once
                if (!leftSignalUI.activeSelf)
                {
                    //Debug.Log("Please turn on your left signal before proceeding.");
                }
                //Stop player movement while left signal is off
                carScript.canMove = false;

            }
            if(lightsScript.leftTurnSignalOn)
            {
                carScript.canMove = true;
               // Debug.Log("Thank you for using your left signal! You may proceed.");
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            carScript.canMove = true;
            leftSignalUsed++;
            dbManager.UsedMergeSignal(leftSignalUsed);
        }
    }
    void HideLeftSignalUI()
    {
        if (leftSignalUI != null)
        {
            leftSignalUI.SetActive(false);
        }
    }
}