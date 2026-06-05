using UnityEngine;

public class TowerInventory : MonoBehaviour
{
    [Header("Master Vault Storage")]
    public int totalScrapStored = 0;
    
    [Header("Dynamic Milestone Scaling")]
    public int scrapRequiredForUpgrade = 7; 
    public int costIncreasePerLevel = 2;    

    [Header("MANUAL LINK SETUP")]
    public UpgradeManager upgradeManager; 

    public void DepositScrap(int amount)
    {
        totalScrapStored += amount;

        if (totalScrapStored >= scrapRequiredForUpgrade)
        {
            totalScrapStored -= scrapRequiredForUpgrade;
            scrapRequiredForUpgrade += costIncreasePerLevel; 

            if (upgradeManager != null)
            {
                upgradeManager.OpenUpgradeMenu();
            }
        }
    }
}