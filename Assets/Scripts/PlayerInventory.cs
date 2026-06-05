using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Player Carrying Stats")]
    public int currentScrap = 0;
    public int maxCarryCapacity = 10;

    public bool AddScrap(int amount)
    {
        if (currentScrap >= maxCarryCapacity)
        {
            currentScrap = maxCarryCapacity;
            
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
            
            return true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DepositZone"))
        {
            if (currentScrap > 0)
            {
                TowerInventory towerVault = other.GetComponent<TowerInventory>();
                if (towerVault != null)
                {
                    towerVault.DepositScrap(currentScrap);
                    currentScrap = 0; 
                }
            }
        }
    }
}