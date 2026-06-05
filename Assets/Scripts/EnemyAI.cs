using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Base Enemy Stats")]
    public float baseSpeed = 2.5f;
    public float currentSpeed;
    public float maxHealth = 3f;
    public float currentHealth;
    public float damage = 2f; 
    public AudioClip deathSoundClip;

    [Header("Drop Settings")]
    public GameObject scrapPrefab; // Drag your Scrap box prefab here in the inspector

    private Transform targetTower;
    private TowerHealth towerHealth;

    void Awake()
    {
        // Initialize current stats with base values
        currentSpeed = baseSpeed;
        currentHealth = maxHealth;
    }

    void Start()
    {
        GameObject tower = GameObject.FindGameObjectWithTag("Tower");
        if (tower != null) {
            targetTower = tower.transform;
            towerHealth = tower.GetComponent<TowerHealth>();
        }
    }

    void Update()
    {
        if (targetTower != null)
        {
            Vector3 targetPosition = new Vector3(targetTower.position.x, transform.position.y, targetTower.position.z);
            
            // Use currentSpeed instead of a hardcoded value
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            }
        }
    }

    // This function is called by the Spawner immediately upon instantiating
    public void ScaleEnemyStats(float healthMultiplier, float speedMultiplier)
    {
        currentHealth = maxHealth * healthMultiplier;
        currentSpeed = baseSpeed * speedMultiplier;

        // Visual check for debugging your scaling in the editor console
        Debug.Log($"{gameObject.name} Spawned with HP: {currentHealth} | Spd: {currentSpeed}");
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        
        Debug.Log($"{gameObject.name} took {amount} damage! HP left: {currentHealth}");

        if (currentHealth <= 0)
        {
            // Drop the loot right before destroying the enemy object
            DropScrap();
            if (deathSoundClip != null)
            {
                AudioSource.PlayClipAtPoint(deathSoundClip, transform.position, 1.0f);
            }
            Destroy(gameObject);
        }
    }

    void DropScrap()
    {
        if (scrapPrefab != null)
        {
            // 1. Calculate the spawn position slightly above the sand
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            
            // 2. Instantiate the scrap completely fresh with zero rotational distortion
            GameObject spawnedScrap = Instantiate(scrapPrefab, spawnPosition, Quaternion.identity);
            
            // 3. ENFORCE GLOBAL INDEPENDENT SCALE
            // This detaches it from any weird inherited scale trends from the dying enemy
            spawnedScrap.transform.parent = null; 
            spawnedScrap.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Forces it to exactly (1, 1, 1)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            // TODO: Deal damage to tower health
            if (towerHealth!=null)
            {
                towerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}