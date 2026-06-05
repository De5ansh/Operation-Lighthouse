using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required if you decide to link a progress slider bar later!
using TMPro;         // Required for showing the loading screen text elements
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Setup")]
    [Tooltip("The build index number of your gameplay scene from the Build Settings window")]
    public int gameplaySceneBuildIndex = 1;

    [Header("Loading UI Elements")]
    public GameObject loadingPanelObject; // Drag your hidden Loading Screen Panel here!
    public TMP_Text loadingText;           // Optional: Drag your text element here to update text prompts
    public Slider loadingProgressBar;      // Optional: Drag a loading slider bar here if you have one

    // Linked to the OnClick() event of your PLAY Button
    public void PlayGame()
    {
        // Kick off the background processing thread
        StartCoroutine(LoadSceneWithTimer());
    }

    private IEnumerator LoadSceneWithTimer()
    {
        // 1. Unfreeze the time scale state just in case
        Time.timeScale = 1f;

        // 2. Turn on the Loading Screen Canvas overlay container
        if (loadingPanelObject != null)
        {
            loadingPanelObject.SetActive(true);
        }

        float totalWaitTime = 5f; // Hard coded target duration requirement
        float currentTimer = 0f;

        // 3. Keep updating the visual graphics for exactly 5 seconds
        while (currentTimer < totalWaitTime)
        {
            currentTimer += Time.deltaTime;
            float normalizedProgress = Mathf.Clamp01(currentTimer / totalWaitTime);

            // Update loading slider bar length if assigned
            if (loadingProgressBar != null)
            {
                loadingProgressBar.value = normalizedProgress;
            }

            // Update loading text display percentage metrics
            if (loadingText != null)
            {
                loadingText.text = $"Deploying Assets... {Mathf.RoundToInt(normalizedProgress * 100f)}%";
            }

            // Wait until the next frame runs before calculating loop numbers again
            yield return null; 
        }

        Debug.Log($"Operation Lighthouse: Loading complete. Booting Build Index {gameplaySceneBuildIndex}...");

        // 4. Time is up! Direct the engine to open up your gameplay map
        SceneManager.LoadScene(gameplaySceneBuildIndex);
    }

    // Linked to the OnClick() event of your QUIT Button
    public void QuitGame()
    {
        Debug.Log("Operation Lighthouse: Shutting down deployment application.");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}