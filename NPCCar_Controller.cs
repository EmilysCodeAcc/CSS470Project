using UnityEngine;

public class NPC_CarController : MonoBehaviour
{
    public float speed = 2f;      
    public bool move = true;    
    private Transform targetPoint;

    void Update()
    {
        if (move)
        {
            //move forward each frame at a constant speed
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurnPoint"))
        {
          transform.rotation = other.transform.rotation; // Rotate the car to match the rotation of the turn point
        }
    }
}