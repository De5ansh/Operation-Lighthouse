using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using TMPro;         
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Setup")]
    [Tooltip("The build index number of your gameplay scene from the Build Settings window")]
    public int gameplaySceneBuildIndex = 1;

    [Header("Loading UI Elements")]
    public GameObject loadingPanelObject; 
    public TMP_Text loadingText;           
    public Slider loadingProgressBar;     

    public void PlayGame()
    {
        StartCoroutine(LoadSceneWithTimer());
    }

    private IEnumerator LoadSceneWithTimer()
    {
        Time.timeScale = 1f;

        if (loadingPanelObject != null)
        {
            loadingPanelObject.SetActive(true);
        }

        float totalWaitTime = 5f; 
        float currentTimer = 0f;

        while (currentTimer < totalWaitTime)
        {
            currentTimer += Time.deltaTime;
            float normalizedProgress = Mathf.Clamp01(currentTimer / totalWaitTime);

            if (loadingProgressBar != null)
            {
                loadingProgressBar.value = normalizedProgress;
            }

            if (loadingText != null)
            {
                loadingText.text = $"Deploying Assets... {Mathf.RoundToInt(normalizedProgress * 100f)}%";
            }

            yield return null; 
        }

        SceneManager.LoadScene(gameplaySceneBuildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Operation Lighthouse: Shutting down deployment application.");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}