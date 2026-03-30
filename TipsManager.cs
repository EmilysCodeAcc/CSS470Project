using UnityEngine;

public class TipsManager : MonoBehaviour
{
    public GameObject tipsPanel;
    public GameObject car;
    public GameObject NPCcar;
    public GameObject levelEnd;
    public GameObject nameScreen;

    public GameObject UI;
    private bool gameActive = false;

    void Start()
    {
        tipsPanel.SetActive(true);
        car.SetActive(false);
        NPCcar.SetActive(false);
        levelEnd.SetActive(false);
        nameScreen.SetActive(false);
        UI.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0) && !gameActive)
        {
            tipsPanel.SetActive(false);
            car.SetActive(true);
            NPCcar.SetActive(true);
            levelEnd.SetActive(true);
            nameScreen.SetActive(true);
            UI.SetActive(true);
            gameActive = true;
        }
    }
}
