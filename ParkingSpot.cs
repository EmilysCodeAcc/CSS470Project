using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Detect Accuracy of how well car is parked forward-facing into parking spot
public class ParkingSpot : MonoBehaviour
{
    public float NeededAngle = 45f; // Angle in degrees that the car needs to be aligned with
    public float AccuracyThreshold = 0f; // Accuracy of car parking
    public float passingThreshold = 0.60f; // Minimum accuracy to be considered parked successfully

    public bool isCarParked = false;
    public bool hasPassed = false;
    public bool stopped = false;
    private Car car;


   private void OnTriggerStay(Collider other)
    {
      if(!other.CompareTag("Player"))
        {
            return;
        }
        //get the rigidbody of the car
       Rigidbody rb = other.attachedRigidbody;
       if(rb == null)
        {
            return;
        }

        //find alignment accuracy
        //take current angle difference between car and parking spot
        float angle = Vector3.Angle(transform.forward, other.transform.forward);
        //calculate alignment score (0 to 1)
        float alignmentScore = Mathf.Clamp01(1f - (angle / NeededAngle));

        //find position accuracy
        Vector3 localPos = transform.InverseTransformPoint(other.transform.position);
        
        //take size of parking spot collider box
        BoxCollider box = GetComponent<BoxCollider>();
        //distance from center to edge on x axis
        float halfLength = box.size.x * 0.5f;
        //calculate horizontal position accuracy
        float posScore = 1f - (Mathf.Abs(localPos.x) / halfLength);

        //calaculate depth accuracy
        float halfDepth = box.size.z * 0.5f;
        float depthScore = 1f - (Mathf.Abs(localPos.z) / halfDepth);
        depthScore = Mathf.Clamp01(depthScore);

        posScore = Mathf.Clamp01(posScore);
        //AccuracyThreshold = (alignmentScore + posScore) / 2f;
        //score an average using alignment, position, and depth
        AccuracyThreshold = (alignmentScore * 0.3f + posScore * 0.4f + depthScore * 0.3f);


       // Debug.Log("Angle: " + angle);
        // Debug.Log("AlignmentScore: " + alignmentScore);
       // Debug.Log("PosScore: " + posScore);
        // Debug.Log("Accuracy: " + AccuracyThreshold);
        
        //get speed of car, if less than 1f considered stopped
        float speed = rb.linearVelocity.magnitude;
        if(speed < .2f)
        {
            stopped = true;
        }
        else
        {
            stopped = false;
        }
        

        if(AccuracyThreshold > passingThreshold && !hasPassed && stopped == true) //60% accuracy or better = parked
        {
            //parked successfully
            hasPassed = true;
            Debug.Log("Car Parked Successfully!");
            car.ParkedSuccessfully(AccuracyThreshold);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCarParked = true;
            car = other.GetComponentInParent<Car>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCarParked = false;
        }
    }

   
}
