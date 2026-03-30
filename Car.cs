using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class Car : MonoBehaviour
{
    public Rigidbody rigid;
    public bool canMove = true;
    public WheelCollider wheel1, wheel2, wheel3, wheel4;
    public float drivespeed, steerspeed;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI wrongLaneCountText;
    public PerformanceSummary performanceSummary;
    float horizontalInput, verticalInput;
    private int count;

    public GameObject levelEnd;


    private int maxSpeed = 4;
    private DataBaseManager dbManager;

    private bool isCorrectLane;
    private int wrongLaneCount;
    public TextMeshProUGUI laneIndicatorText;

    public int redLightViolationCount = 0;

    public Transform SpawnPoint;
    public float parkingAccuracy = 0f;
    public bool hasParked = false;
    public int GetRedLightViolationCount()
    {
        return redLightViolationCount;
    }

    public int GetCollisionCount()
{
    return count;
}

    public float GetCurrentSpeed()
    {
        return rigid.linearVelocity.magnitude * 3.6f;
    }
    public int GetWrongLaneCount()
    {
        return wrongLaneCount;
    }

    void Start()
    {
        levelEnd.SetActive(false);
        count = 0;
        SetCountText();
        showSpeed();
        wrongLaneCount = 0;
        SetWrongLaneCountText();

        dbManager = FindFirstObjectByType<DataBaseManager>();
        if (dbManager != null)
        {
            StartCoroutine(UpdateFirebaseStats());
        }else{Debug.LogWarning("DataBaseManager not found in the scene.");}
    }

    // Update is called once per frame
    void Update()
    {
        //control car movement
        if(!canMove)
        {
            wheel1.motorTorque = 0;
            wheel2.motorTorque = 0;
            wheel3.motorTorque = 0;
            wheel4.motorTorque = 0;

            rigid.linearVelocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            return;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        UpdateLaneIndicator();
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton5)) //Input for right bumper on controller
        {
            resetCar();
        }
    }
    void FixedUpdate()
    {
        float motor = Input.GetAxis("Vertical") * drivespeed;
        wheel1.motorTorque = motor;
        wheel3.motorTorque = motor;
        wheel2.motorTorque = motor;
        wheel4.motorTorque = motor;
        wheel1.steerAngle = steerspeed * horizontalInput;
        wheel2.steerAngle = steerspeed * horizontalInput;
        showSpeed();
        limitSpeed();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WallCol")||other.gameObject.CompareTag("NPCCol"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        if (other.gameObject.CompareTag("CorrectLane"))
        {
            isCorrectLane = true;
            UpdateLaneIndicator();
        }
        if (other.gameObject.CompareTag("WrongLane"))
        {   
            isCorrectLane = false;
            wrongLaneCount = wrongLaneCount + 1;
            SetWrongLaneCountText();
            UpdateLaneIndicator();
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("EndCol"))
        {
            levelEnd.SetActive(true);
            if (performanceSummary != null)
            {
                performanceSummary.OnLevelEnd();
            }
            else {
                Debug.LogWarning("PerformanceSummary component not found on the Car object."); 
                 }
            
        }
        if (other.CompareTag("CorrectLane"))
        {
            isCorrectLane = false;
            wrongLaneCount = wrongLaneCount + 1;
            SetWrongLaneCountText();
            UpdateLaneIndicator();
        }
        if (other.CompareTag("WrongLane"))
        {
            isCorrectLane = true; 
            UpdateLaneIndicator();
        }
        
    }
    public void ParkedSuccessfully(float accuracy)
{
    if (hasParked) return; // prevent multiple triggers
    float speed = GetCurrentSpeed();
    hasParked = true;
    parkingAccuracy = accuracy;
    

    Debug.Log("Parking Complete! Accuracy: " + (accuracy * 100f).ToString("F0") + "%");
     // Immediately write to Firebase BEFORE summary is generated
    if (dbManager != null)
    {
        string redLightVal = redLightViolationCount.ToString();
        string collisionValue = count.ToString();
        string speedValue = GetCurrentSpeed().ToString("F0");
        string wrongLaneCountValue = wrongLaneCount.ToString();
        string parkingAccuracyValue = parkingAccuracy.ToString("F2");

        dbManager.UpdateMetrics(
            collisionValue,
            speedValue,
            wrongLaneCountValue,
            redLightVal,
            parkingAccuracyValue
        );
    }


    //levelEnd.SetActive(true);
    new WaitForSeconds(5f);
    levelEnd.SetActive(true);
    if (performanceSummary != null)
    {
        performanceSummary.OnLevelEnd();
    }
    
}

    //show metrics in UI text fields
    void SetCountText()
    {
        countText.text = "Collisions: " + count.ToString();
    }
    void SetWrongLaneCountText()
    {
        wrongLaneCountText.text = "Wrong Lane Count: " + wrongLaneCount.ToString();
    }
    void showSpeed()
    {
        float speed = rigid.linearVelocity.magnitude * 3.6f;
        speedText.text = "Speed: " + speed.ToString("F0") + " km/h";
    }
    
    //cap the speed of the car to the max speed
    void limitSpeed()
    {
        if (rigid.linearVelocity.magnitude > maxSpeed)
        {
            rigid.linearVelocity = rigid.linearVelocity.normalized * maxSpeed;
        }
    }

    //Write to firebase every 2 seconds with the current collision count, speed, and wrong lane count
    IEnumerator UpdateFirebaseStats()
    {
        while (true)
        {
            if (dbManager != null)
            {
                string redLightVal = redLightViolationCount.ToString();
                string collisionValue = count.ToString();
                string speedValue = (rigid.linearVelocity.magnitude * 3.6f).ToString("F0");
                string wrongLaneCountValue = wrongLaneCount.ToString();
                string parkingAccuracyValue = parkingAccuracy.ToString("F2");

                dbManager.UpdateMetrics(collisionValue, speedValue, wrongLaneCountValue, redLightVal, parkingAccuracyValue);
            }
            yield return new WaitForSeconds(5f); // update every 2 seconds
        }
    }
   

    void UpdateLaneIndicator()
    {
        if (isCorrectLane)
        {
            laneIndicatorText.text = "Correct Lane!";
        }
        else
        {
            laneIndicatorText.text = "Wrong Lane!";
        }
    }
    void resetCar()
    {
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        rigid.isKinematic = true; // Disable physics temporarily
        transform.position = SpawnPoint.position + Vector3.up * 1f; // Move the player slightly above the spawn point to avoid collision issues
        transform.rotation = SpawnPoint.rotation; // Reset rotation to spawn point's rotation
        rigid.isKinematic = false; // Re-enable physics
    }

}
