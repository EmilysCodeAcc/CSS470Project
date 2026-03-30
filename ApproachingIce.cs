using UnityEngine;
using UnityEngine.UI;
public class ApproachingIce : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject warningUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (warningUI != null)
            warningUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (warningUI != null)
            {
                warningUI.SetActive(true);
                Invoke("HideWarning", 5f);
            }
        }
    }

    void HideWarning()
    {
        if (warningUI != null)
            warningUI.SetActive(false);
    }
}