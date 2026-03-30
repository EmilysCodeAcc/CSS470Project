using UnityEngine;

public class MCDetect : MonoBehaviour
{
    //detect when playuer collides with oncoming vehicle and respawn at spawn point
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Collision with MergeVehicle detected. Respawning player.");

        Rigidbody playerRb = other.attachedRigidbody;
        if (playerRb == null)
        {
            Debug.LogWarning("Player has no Rigidbody!");
            return;
        }

        RespawnPlayer(playerRb);
    }

    private void RespawnPlayer(Rigidbody playerRb)
    {
        // Stop motion
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        // Temporarily disable physics
        playerRb.isKinematic = true;

        // Move player
        playerRb.transform.position = spawnPoint.position + Vector3.up * 1f;
        playerRb.transform.rotation = spawnPoint.rotation;

        // Re-enable physics
        playerRb.isKinematic = false;
    }
}