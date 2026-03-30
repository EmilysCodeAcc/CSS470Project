using UnityEngine;

public class CarMovementController2 : MonoBehaviour
{
    public float speed = 2f;      
    public bool move = true;    
    private Transform targetPoint;

    void Update()
    {
        if (move)
        {
            //move backward each frame at a constant speed
            transform.Translate(Vector3.back * speed * Time.deltaTime);
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