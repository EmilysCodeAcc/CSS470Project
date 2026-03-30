using UnityEngine;

public class GameExit : MonoBehaviour
{
    /*void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }*/
    public void  LoadLevel(string levelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

}
