using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndScene : MonoBehaviour
{
    [SerializeField] private GameObject endScreenUI;
    
    
    [SerializeField] private TextMeshProUGUI bigText;
    [SerializeField] private TextMeshProUGUI smallText;

    public void LoadEndScreenVictory()
    {
        Time.timeScale = 0;
        endScreenUI.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        
        bigText.text = "Victory!";
        smallText.text = "You have successfully reached the end!";
    }
    
    public void LoadEndScreenLoss()
    {
        Time.timeScale = 0;
        endScreenUI.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        bigText.text = "You Died";
        smallText.text = "Next time don't get spotted so easily";
    }
    
    
    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
