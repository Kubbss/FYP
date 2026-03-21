using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static bool gameOver;

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
        
        gameOver = true;
        
        LoadLossScene();
    }

    public void PlayerEscape()
    {
        LoadSuccessScene();
    }

    private void LoadLossScene()
    {
        gameOver = true;
        
        GameObject temp = GameObject.FindGameObjectWithTag("PlayerUI");
        temp.GetComponent<GameEndScene>().LoadEndScreenLoss();
        
    }
    
    private void LoadSuccessScene()
    {
        gameOver = true;
        
        GameObject temp = GameObject.FindGameObjectWithTag("PlayerUI");
        temp.GetComponent<GameEndScene>().LoadEndScreenVictory();
    }
}
