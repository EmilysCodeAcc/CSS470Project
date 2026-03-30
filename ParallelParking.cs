using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Detect Accuracy of how well car is parallel-parked into parking spot

public class ParallelParking : MonoBehaviour
{
    public float maxAngle = 30f; // Maximum angle difference for parallel parking
    public float maxDistance = 2f; // Maximum distance from parking spot center for parallel parking
    public float accuracyThreshold = 0.65f; // Accuracy of car parking
    public Transform frontCarPoint; // Point at the front of the car
    public Transform backCarPoint; // Point at the back of the car
    private bool hasPassed = false;
    private Car car;
   public void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        //get the rigidbody of the car
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null)
        {
            return;
        }

        Vector3 localPosition = transform.InverseTransformPoint(other.transform.position);
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        
        float halfDepth = boxCollider.size.z / 2f;

        //find alignment
        float angle = Vector3.Angle(transform.forward, other.transform.forward);
        float alignmentScore = Mathf.Clamp01(1f - (angle / maxAngle));

        //side position
        float sideOffset = Mathf.Abs(localPosition.x);
        float sideScore = Mathf.Clamp01(1f - (sideOffset/maxDistance));

        //front and back position
        float forwardOffsetFront = Mathf.Abs(localPosition.z);
        float forwardScore = Mathf.Clamp01(1f-(forwardOffsetFront/halfDepth));

        //clearance between front and back of car
        float frontDistance = Vector3.Distance(other.transform.position, frontCarPoint.position);
        float backDistance = Vector3.Distance(other.transform.position, backCarPoint.position);

        float nearestDistance = Mathf.Min(frontDistance, backDistance);
        float clearanceScore = Mathf.InverseLerp(0.5f, 2.5f, nearestDistance);

        //final score
        float finalScore = ((alignmentScore* 0.35f) + (sideScore*0.35f) + (forwardScore*0.20f) + (clearanceScore*0.10f));
        if (finalScore >= accuracyThreshold && !hasPassed)
        {
            hasPassed = true;
            Debug.Log("Parallel Parking Complete! Score: " + finalScore);

            car.ParkedSuccessfully(finalScore);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
             car = other.GetComponentInParent<Car>();
             if (car == null)
            {
                Debug.LogError("ParallelParking: Car script NOT FOUND on Player object!");
            }

            hasPassed = false;
        }
    }

}
