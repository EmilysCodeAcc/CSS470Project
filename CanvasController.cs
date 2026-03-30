using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject targetCanvas;
    void Start()
    {
        targetCanvas.SetActive(false);
    }

    public void ShowCanvas()
    {
        targetCanvas.SetActive(true);
    }
}
