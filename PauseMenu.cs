using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   public static bool GameIsPaused = false;
   public GameObject pauseMenuUI;
   public GameObject PlayerUI;
   void Update()
   {
    //when escape key is pressed, pause or resume game
       if (Input.GetKeyDown(KeyCode.Escape))
       {
           if (GameIsPaused)
           {
               Resume();
           }
           else
           {
               Pause();
           }
       }
   }
   void Resume()
   {
       //enable UI elements
       pauseMenuUI.SetActive(false);
       PlayerUI.SetActive(true);
       Time.timeScale = 1f;
       GameIsPaused = false;
   }
   void Pause()
   {   
        //disable UI elements
       pauseMenuUI.SetActive(true);
       PlayerUI.SetActive(false);
       Time.timeScale = 0f;
       GameIsPaused = true;
   }
}
