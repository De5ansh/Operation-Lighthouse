using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject upgradeCanvasObject; // Drag UpgradeUICanvas here
    public TMP_Text[] buttonTexts;         // Drag the Text components of Button 1, 2, and 3 here

    [Header("Mini Towers To Activate")]
    public GameObject smallTower1;        // Drag your unchecked Mini-Tower 1 object here
    public GameObject smallTower2;        // Drag your unchecked Mini-Tower 2 object here

    [Header("MANUAL GAME OBJECT LINKS")]
    // NEW: Manual drag-and-drop slots instead of using slower automatic code finding!
    public TowerWeapon mainTowerWeapon;     // Drag your Central Main Tower here
    public PlayerController playerMovement; // Drag your Player object here
    public PlayerInventory playerInventory;   // Drag your Player object here too

    // Core upgrade tracker
    private enum UpgradeType { FireRate, AttackRange, PlayerSpeed, ProjectileDamage, MiniTower, PlayerCapacity, MiniTowerDamage }
    private List<UpgradeType> currentChoices = new List<UpgradeType>();

    // Cached weapon components for the mini towers
    private TowerWeapon smallTower1Weapon;
    private TowerWeapon smallTower2Weapon;

    void Start()
    {
        // Cache the weapon components from your mini tower variables safely
        if (smallTower1 != null) smallTower1Weapon = smallTower1.GetComponent<TowerWeapon>();
        if (smallTower2 != null) smallTower2Weapon = smallTower2.GetComponent<TowerWeapon>();

        // SAFETY CHECK: Warns you instantly in the editor if you forgot a critical link!
        if (mainTowerWeapon == null || playerMovement == null || playerInventory == null)
        {
            Debug.LogError("CRITICAL: You forgot to drag the Main Tower or Player references into the UpgradeManager Inspector fields!");
        }
    }

    public void OpenUpgradeMenu()
    {
        Time.timeScale = 0f; 
        upgradeCanvasObject.SetActive(true);
        GenerateRandomUpgrades();
    }

    void GenerateRandomUpgrades()
    {
        currentChoices.Clear();
        List<UpgradeType> availablePool = new List<UpgradeType>();

        // 1. FILTER POOL: Build deck based on direct, reliable object references
        if (mainTowerWeapon != null && mainTowerWeapon.fireRate < 5f)
            availablePool.Add(UpgradeType.FireRate);

        if (mainTowerWeapon != null && mainTowerWeapon.attackRange < 25f)
            availablePool.Add(UpgradeType.AttackRange);

        if (playerMovement != null && playerMovement.baseSpeed < 12f)
            availablePool.Add(UpgradeType.PlayerSpeed);

        if (mainTowerWeapon != null && mainTowerWeapon.baseDamage < 10f)
            availablePool.Add(UpgradeType.ProjectileDamage);

        if (playerInventory != null && playerInventory.maxCarryCapacity < 30)
            availablePool.Add(UpgradeType.PlayerCapacity);

        // Mini tower validation switch
        bool tower1Active = smallTower1 != null && smallTower1.activeSelf;
        bool tower2Active = smallTower2 != null && smallTower2.activeSelf;

        if (!tower1Active || !tower2Active)
        {
            availablePool.Add(UpgradeType.MiniTower);
        }
        else
        {
            bool tower1UnderCap = smallTower1Weapon != null && smallTower1Weapon.baseDamage < 8f;
            bool tower2UnderCap = smallTower2Weapon != null && smallTower2Weapon.baseDamage < 8f;

            if (tower1UnderCap || tower2UnderCap)
            {
                availablePool.Add(UpgradeType.MiniTowerDamage);
            }
        }

        // 2. GENERATE THE CARDS ON SCREEN
        for (int i = 0; i < 3; i++)
        {
            UnityEngine.UI.Button buttonComponent = buttonTexts[i].gameObject.GetComponentInParent<UnityEngine.UI.Button>();
            if (buttonComponent != null) buttonComponent.interactable = true;
            buttonTexts[i].gameObject.transform.parent.gameObject.SetActive(true);

            if (availablePool.Count > 0)
            {
                int randomIndex = Random.Range(0, availablePool.Count);
                UpgradeType selected = availablePool[randomIndex];
                
                currentChoices.Add(selected);
                buttonTexts[i].text = FormatUpgradeName(selected);
                
                availablePool.RemoveAt(randomIndex); 
            }
            else
            {
                currentChoices.Add(UpgradeType.ProjectileDamage); 
                buttonTexts[i].text = "Overcharged Impact (Baseline Maxed Damage Boost)";
            }
        }
    }

    string FormatUpgradeName(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.FireRate: return "Tower Fire Rate +20%";
            case UpgradeType.AttackRange: return "Tower Radar Range +15%";
            case UpgradeType.PlayerSpeed: return "Supplier Run Speed +15%";
            case UpgradeType.ProjectileDamage: return "Main Tower Damage +1";
            case UpgradeType.MiniTower: return "Activate Auxiliary Mini-Turret";
            case UpgradeType.PlayerCapacity: return "Backpack Pocket Capacity +5";
            case UpgradeType.MiniTowerDamage: return "Mini-Turret Damage +1";
            default: return "Upgrade";
        }
    }

    public void SelectUpgradeOption(int buttonIndex)
    {
        UpgradeType chosenUpgrade = currentChoices[buttonIndex];

        switch (chosenUpgrade)
        {
            case UpgradeType.FireRate:
                if (mainTowerWeapon != null)
                    mainTowerWeapon.fireRate = Mathf.Min(mainTowerWeapon.fireRate * 1.20f, 5f);
                break;

            case UpgradeType.AttackRange:
                if (mainTowerWeapon != null)
                    mainTowerWeapon.attackRange = Mathf.Min(mainTowerWeapon.attackRange * 1.15f, 25f);
                break;

            case UpgradeType.PlayerSpeed:
                if (playerMovement != null)
                    playerMovement.baseSpeed = Mathf.Min(playerMovement.baseSpeed * 1.15f, 12f);
                break;

            case UpgradeType.ProjectileDamage:
                if (mainTowerWeapon != null)
                    mainTowerWeapon.baseDamage = Mathf.Min(mainTowerWeapon.baseDamage + 1f, 10f);
                break;
                
            case UpgradeType.MiniTower:
                if (smallTower1 != null && !smallTower1.activeSelf)
                    smallTower1.SetActive(true);
                else if (smallTower2 != null && !smallTower2.activeSelf)
                    smallTower2.SetActive(true);
                break;

            case UpgradeType.PlayerCapacity:
                if (playerInventory != null)
                    playerInventory.maxCarryCapacity = Mathf.Min(playerInventory.maxCarryCapacity + 5, 30);
                break;

            case UpgradeType.MiniTowerDamage:
                if (smallTower1Weapon != null)
                    smallTower1Weapon.baseDamage = Mathf.Min(smallTower1Weapon.baseDamage + 1f, 8f);

                if (smallTower2Weapon != null)
                    smallTower2Weapon.baseDamage = Mathf.Min(smallTower2Weapon.baseDamage + 1f, 8f);
                break;
        }

        upgradeCanvasObject.SetActive(false);
        Time.timeScale = 1f; 
    }
}