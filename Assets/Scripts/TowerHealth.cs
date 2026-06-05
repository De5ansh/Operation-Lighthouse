using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI Canvas Connection")]
    public GameOver gameOverScript; 

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0f);

        if (currentHealth <= 0f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        if (gameOverScript != null)
        {
            gameOverScript.DisplayGameOverScreen("The slimes breached the tower!");
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
}