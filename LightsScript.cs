using UnityEngine;
using System.Collections;
//enable and disable car lights
public class LightsScript : MonoBehaviour
{
    [Header("Assign car lights here")]
   public Light[] carLights;
   public Light[] LeftSignal;
   public Light[] RightSignal; 
   private bool lightsOn = false;
   public bool leftTurnSignalOn = false;
   public bool rightTurnSignalOn = false;
   public DataBaseManager dbManager;
   public float blinkInterval = 0.5f;
   private Coroutine leftBlinkCoroutine;
   private Coroutine rightBlinkCoroutine;
   public void Update()
    {
        //When the player presses the L key, toggle the car lights on and off
        if (Input.GetKeyDown(KeyCode.L) ||  Input.GetKeyDown(KeyCode.JoystickButton3)) // Y button input
        {
            ToggleLights();
        }
        if(Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.JoystickButton2)) // X button input for left turn signal
        {
           leftToggleTurnSignal();
        }
        if(Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.JoystickButton1)) //  B button input for right turn signal
        {
            rightToggleTurnSignal();
        }
        dbManager.UpdateLightInfo(lightsOn, leftTurnSignalOn, rightTurnSignalOn);
    }
    //toggle car lights on and off
    public void ToggleLights()
    {
        lightsOn = !lightsOn;
        foreach (Light light in carLights)
        {
            light.gameObject.SetActive(lightsOn);
           
        }
    }
    //toggle turn signals on and off
    public void leftToggleTurnSignal()
    {
        leftTurnSignalOn = !leftTurnSignalOn;
        if (rightTurnSignalOn)
        {
            rightToggleTurnSignal();
        }
       foreach (Light light in LeftSignal)
        {
            light.gameObject.SetActive(leftTurnSignalOn);
        }
        if (leftTurnSignalOn)
        {
            leftBlinkCoroutine = StartCoroutine(BlinkTurnSignal(LeftSignal));
        }
    }
    public void rightToggleTurnSignal()
    {
        rightTurnSignalOn = !rightTurnSignalOn;
        if (leftTurnSignalOn)
        {
            leftToggleTurnSignal();
        }
        foreach (Light light in RightSignal)
        {
            light.gameObject.SetActive(rightTurnSignalOn);
            
        }
        if (rightTurnSignalOn)
        {
            rightBlinkCoroutine = StartCoroutine(BlinkTurnSignal(RightSignal));
        }
    }

    IEnumerator BlinkTurnSignal(Light[] signalLights)
    {
        while (true)
        {
            foreach (Light light in signalLights)
            {
                light.enabled = !light.enabled;
            }
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
