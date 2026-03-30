using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//if player speed is above 10 when near ice, sends them back to spawn point
public class SpeedCatch : MonoBehaviour
{
    public Rigidbody rigid;
    public Transform spawnPoint;
    public float speedThreshold = 10f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ice"))
        {
            float currentSpeed = rigid.linearVelocity.magnitude * 3.6f; 
            Debug.Log("Current Speed: " + currentSpeed + " km/h");
            if (currentSpeed > speedThreshold)
            {
                RespawnPlayer();

            }
        }
    }

    private void RespawnPlayer()
    {
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        rigid.isKinematic = true; // Disable physics temporarily
        transform.position = spawnPoint.position + Vector3.up * 1f; // Move the player slightly above the spawn point to avoid collision issues
        transform.rotation = spawnPoint.rotation; // Reset rotation to spawn point's rotation
        rigid.isKinematic = false; // Re-enable physics
    }

}
