using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public TMP_Text deathText;

    public void DisplayGameOverScreen(string txt)
    {
        Time.timeScale = 0f;
        deathText.text = txt;
        
        gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        yield return new WaitForSecondsRealtime(5f);

        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    public void MainMenu()
    {
        StartCoroutine(MainMenuRoutine());
    }

    private IEnumerator MainMenuRoutine()
    {
        yield return new WaitForSecondsRealtime(5f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(0); 
    }

    public void Quit()
    {
        StartCoroutine(QuitRoutine());
    }

    private IEnumerator QuitRoutine()
    {
        yield return new WaitForSecondsRealtime(5f);

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}