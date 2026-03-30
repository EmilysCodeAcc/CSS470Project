using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public void CloseUserInterface()
    {
        if (gameObject.CompareTag("UI"))
        {
            gameObject.SetActive(false);
        }
    }
}
