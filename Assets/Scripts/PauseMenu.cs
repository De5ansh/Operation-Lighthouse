using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;          
        gameObject.SetActive(true);    
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;          
        gameObject.SetActive(false);   
    }

    public void LoadMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;         
        SceneManager.LoadScene(0);     
    }

    public void Quit()
    {
        Application.Quit();
    }
}