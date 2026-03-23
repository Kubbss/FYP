using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   [SerializeField] GameObject pauseMenuUI;
   [SerializeField] TextMeshProUGUI sensText;
   
   public void Pause()
   {
      pauseMenuUI.SetActive(true);
      Time.timeScale = 0;
      
      Cursor.lockState = CursorLockMode.Confined;
      Cursor.visible = true;
   }

   public void Home()
   {
      Time.timeScale = 1;
      SceneManager.LoadSceneAsync(0);
   }

   public void Resume()
   {
      pauseMenuUI.SetActive(false);
      Time.timeScale = 1;
      
      Cursor.lockState = CursorLockMode.Locked;
      Cursor.visible = false;
   }

   public void Quit()
   {
      Application.Quit();
   }

   public void TogglePause()
   {
      if (GameManager.gameOver)
         return;
      
      if (pauseMenuUI.activeSelf)
      {
         Resume();
      }
      else
      {
         Pause();
      }
   }
   
   public void UpdateSensText(float sens)
   {
      sensText.text = sens.ToString("00");
   }
}
