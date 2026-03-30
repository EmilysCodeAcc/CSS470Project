using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    public GameObject yellowLight;
    public GameObject redLight;
    public GameObject greenLight;
    public enum LightState { Green, Yellow, Red };
    public LightState currentState = LightState.Green;

    public float yellowDuration = 5f;
    public float redDuration = 10f;

    private bool isRunning = false;

    private void Start()
    {
        SetLightState(green: true, yellow: false, red: false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isRunning)
        {
            StartCoroutine(TrafficSequence());
        }
    }

    private System.Collections.IEnumerator TrafficSequence()
    {
        isRunning = true;

        // Yellow phase
        SetLightState(false, true, false);
        yield return new WaitForSeconds(yellowDuration);

        // Red phase
        SetLightState(false, false, true);
        yield return new WaitForSeconds(redDuration);

        // Green phase
        SetLightState(true, false, false);

        isRunning = false;   // allow re-trigger if needed
    }

    public void SetLightState(bool green, bool yellow, bool red)
    {
        greenLight.SetActive(green);
        redLight.SetActive(yellow);
        yellowLight.SetActive(red);
         if (green)
        {
            currentState = LightState.Green;
        }
        else if (yellow)
        {
            currentState = LightState.Yellow;
        }
        else if (red)
        {
            currentState = LightState.Red;
        }
    }
}
