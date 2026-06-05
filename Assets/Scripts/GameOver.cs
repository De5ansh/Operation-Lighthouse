using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections; // CRITICAL: Required for IEnumerator and Coroutines to work!

public class GameOver : MonoBehaviour
{
    public TMP_Text deathText;

    public void DisplayGameOverScreen(string txt)
    {
        // 1. Freeze the game physics engine completely instantly on defeat
        Time.timeScale = 0f;
        deathText.text = txt;
        
        // 2. Turn on this panel object so the text and buttons appear
        gameObject.SetActive(true);
    }

    // --- RESTART BUTTON ---
    // This remains a normal public void so your UI Button component can still see and click it!
    public void RestartGame()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        Debug.Log("Operation Lighthouse: Resetting shoreline deployment in 5 seconds...");
        
        // Wait out a strict 5-second real-world countdown clock
        yield return new WaitForSecondsRealtime(5f);

        // CRITICAL: Unfreeze the time scale state BEFORE reloading the map!
        Time.timeScale = 1f;

        // Safely pull the index integer of whatever scene is open and refresh it
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    // --- MAIN MENU BUTTON ---
    public void MainMenu()
    {
        StartCoroutine(MainMenuRoutine());
    }

    private IEnumerator MainMenuRoutine()
    {
        Debug.Log("Returning to main menu hubs in 5 seconds...");
        yield return new WaitForSecondsRealtime(5f);

        Time.timeScale = 1f;
        SceneManager.LoadScene(0); // Loads scene at index 0 (Your Main Menu)
    }

    // --- QUIT BUTTON ---
    public void Quit()
    {
        StartCoroutine(QuitRoutine());
    }

    private IEnumerator QuitRoutine()
    {
        Debug.Log("Operation Lighthouse: Exiting application in 5 seconds...");
        yield return new WaitForSecondsRealtime(5f);

        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}