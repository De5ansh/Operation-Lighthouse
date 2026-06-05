using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("UI Text Fields")]
    public TMP_Text playerScrapText;
    public TMP_Text towerScrapText;
    public TMP_Text waveText;

    [Header("UI Slider Fill Bar")]
    public Slider healthBarSlider;

    [Header("Game Script Data Connections")]
    public PlayerInventory playerInventory;
    public TowerInventory towerInventory;
    public TowerHealth towerHealth;
    public WaveManager waveManager;

    void Start()
    {
        if (playerInventory == null || towerInventory == null || towerHealth == null)
        {
            Debug.LogError("GameHUD Error: Assign all script data references in the Inspector slots!");
        }
    }

    void Update()
    {

        if (waveManager != null && waveText != null)
        {
            waveText.text = $"Wave {waveManager.waveNumber}";
        }
        if (playerInventory != null && playerScrapText != null)
        {
            playerScrapText.text = $"Scrap Carrying: {playerInventory.currentScrap} / {playerInventory.maxCarryCapacity}";
        }

        if (towerInventory != null && towerScrapText != null)
        {
            towerScrapText.text = $"Tower Vault: {towerInventory.totalScrapStored} / {towerInventory.scrapRequiredForUpgrade}";
        }

        if (towerHealth != null)
        {
            
            if (healthBarSlider != null)
            {
                healthBarSlider.maxValue = towerHealth.maxHealth;
                healthBarSlider.value = towerHealth.currentHealth;
            }
        }
    }
}