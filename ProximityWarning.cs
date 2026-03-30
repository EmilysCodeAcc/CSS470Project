using UnityEngine;
using UnityEngine.UI;

public class ProximityWarning : MonoBehaviour
{
    public string playerTag = "Player";     
    public GameObject warningUI;            

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
            //set active for ten seconds
            {
                warningUI.SetActive(true);  
                Invoke("HideWarning", 5f); 
            }
        }
    }

   /* void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (warningUI != null)
                warningUI.SetActive(false);
        }
    }*/
    
    void HideWarning()
    {
        if (warningUI != null)
            warningUI.SetActive(false);
    }
}