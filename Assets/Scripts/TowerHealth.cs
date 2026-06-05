using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Canvas Connection")]
    // Link directly to the new canvas script component
    public GameOver gameOverScript; 

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);
        Debug.Log($"Tower Hit! Remaining Armor: {currentHealth}");

        if (currentHealth <= 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.LogError("GAME OVER! The slimes breached the tower!");

        if (gameOverScript != null)
        {
            // Tell the canvas script to freeze time and turn itself on!
            gameOverScript.DisplayGameOverScreen("The slimes breached the tower!");
        }
        else
        {
            Time.timeScale = 0f;
            Debug.LogError("TowerHealth Error: gameOverCanvasScript reference is missing!");
        }
    }
}