using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Check player speed when they are in the stop zone, if they are moving too fast during yellow light or red light, log a message
public class StopZone : MonoBehaviour
{
    public TrafficLightController trafficLight;
    public float maxAllowedSpeed = 0.5f;      
    public string playerTag = "Player";    
    public Transform spawnPoint;   
    public int stopZoneViolationCount = 0;

    //use car component to pass traffic violation count to database manager
    private Car car;
    private bool playerStoppedCorrectly = false;

    
    private void OnTriggerStay(Collider other)
    {
        //debug to find if player is in the stop zone
        //Debug.Log("Something is inside the StopZone: " + other.name);
        //check that the object is the player
        if (!other.CompareTag(playerTag)) return;
        
        if (car == null){
            car = other.GetComponentInParent<Car>();
            if (car == null)
            {
                Debug.LogWarning("Player does not have a Car component.");
                return;
            }
        }
        
        if (other.CompareTag(playerTag))
        {
            // Get the Rigidbody component of the player
            Rigidbody rb = other.attachedRigidbody;
            if(rb == null)
            {
                Debug.LogWarning("Player does not have a Rigidbody component.");
                return;
            }
             if (trafficLight.currentState == TrafficLightController.LightState.Yellow)
            {
                //check the players speed and log if they are moving too fast during yellow light
                if (rb.linearVelocity.magnitude > maxAllowedSpeed)
                {
                    Debug.Log("Player is moving too fast during Yellow light!");
                }
            }

            // Check if light is red
            if (trafficLight.currentState == TrafficLightController.LightState.Red)
            {
                // Check speed of the player
                if (rb.linearVelocity.magnitude <= maxAllowedSpeed)
                {
                    playerStoppedCorrectly = true;
                }
                else 
                {
                    playerStoppedCorrectly = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag(playerTag))
        {
              if (car == null){
                 car = other.GetComponent<Car>();
              }
            if (trafficLight.currentState == TrafficLightController.LightState.Red)
            {
                // Check if player stopped correctly during the red light
                if (playerStoppedCorrectly){
                    Debug.Log("Player FOLLOWED the traffic rule!");
                }

                else{
                    Debug.Log("Player RAN the red light!");
                    if (car!=null)
                    {
                        car.redLightViolationCount++;
                    }
                    RespawnPlayer(other.attachedRigidbody);
                }

                playerStoppedCorrectly = false;
            }
        }
    }

    private void RespawnPlayer(Rigidbody playerRB)
    {
        playerRB.linearVelocity = Vector3.zero;
        playerRB.angularVelocity = Vector3.zero;

       // playerRB.isKinematic = true; // Disable physics temporarily
        playerRB.transform.position = spawnPoint.position + Vector3.up * 1f; // Move the player slightly above the spawn point to avoid collision issues
        transform.rotation = spawnPoint.rotation; // Reset rotation to spawn point's rotation
        //playerRB.isKinematic = false; // Re-enable physics
    }
  
}