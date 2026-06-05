using UnityEngine;

public class TowerInventory : MonoBehaviour
{
    [Header("Master Vault Storage")]
    public int totalScrapStored = 0;
    
    [Header("Dynamic Milestone Scaling")]
    public int scrapRequiredForUpgrade = 7; 
    public int costIncreasePerLevel = 2;    

    [Header("MANUAL LINK SETUP")]
    // FIX: Making this public forces a slot to appear in the Inspector!
    public UpgradeManager upgradeManager; 

    public void DepositScrap(int amount)
    {
        totalScrapStored += amount;
        Debug.Log($"Tower Stored Bank Vault: {totalScrapStored} Scrap. Target Goal: {scrapRequiredForUpgrade}");

        if (totalScrapStored >= scrapRequiredForUpgrade)
        {
            totalScrapStored -= scrapRequiredForUpgrade;
            scrapRequiredForUpgrade += costIncreasePerLevel; 

            Debug.Log($"Milestone Cleared! Opening Upgrade Menu now...");

            if (upgradeManager != null)
            {
                upgradeManager.OpenUpgradeMenu();
            }
            else
            {
                // If you forgot to drag the script in, this error will point it out instantly!
                Debug.LogError("CRITICAL: You forgot to drag the UpgradeUICanvas into the Tower Inventory slot!");
            }
        }
    }
}