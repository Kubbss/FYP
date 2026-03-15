using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void OptionsButton()
    {
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index);
    }
}
