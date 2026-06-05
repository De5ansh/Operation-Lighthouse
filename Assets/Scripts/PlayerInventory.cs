using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player Carrying Stats")]
    public int currentScrap = 0;
    public int maxCarryCapacity = 10;

    // REMOVED: towerVault field and Start() function are completely gone!

    public bool AddScrap(int amount)
    {
        if (currentScrap >= maxCarryCapacity)
        {
            currentScrap = maxCarryCapacity;
            
            Debug.Log("Pockets full! Go to the flat sphere zone to unload!");
            return false;
        }
        else
        {
            if (currentScrap + amount > maxCarryCapacity)
            {
                currentScrap = maxCarryCapacity;
            } else
            {
                currentScrap += amount;
            }
            
            Debug.Log($"Inventory: {currentScrap} / {maxCarryCapacity}");
            return true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect if we stepped onto the flattened sphere zone boundary
        if (other.CompareTag("DepositZone"))
        {
            if (currentScrap > 0)
            {
                // DYNAMIC LINK: Grab the TowerInventory component straight off the object we just bumped into!
                // (This assumes your TowerInventory script is attached directly to the deposit zone or its main parent)
                TowerInventory towerVault = other.GetComponent<TowerInventory>();

                // FALLBACK: If the script is on the main parent Tower object instead of the flat sphere child, check the parent!

                if (towerVault != null)
                {
                    Debug.Log($"Depositing {currentScrap} scrap into the tower base...");
                    
                    // Transfer the player's current stash to the tower's bank account
                    towerVault.DepositScrap(currentScrap);
                    
                    // Reset player's capacity back to empty
                    currentScrap = 0; 
                }
                else
                {
                    Debug.LogError("Player stepped on DepositZone, but couldn't find a TowerInventory script on it or its parents!");
                }
            }
        }
    }
}