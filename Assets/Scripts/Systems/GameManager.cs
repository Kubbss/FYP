using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void KillPlayer()
    {
        Debug.Log("Player Died");
        LoadLossScene();
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadLossScene()
    {
        Time.timeScale = 0;
    }
}
