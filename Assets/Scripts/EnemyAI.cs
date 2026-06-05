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
    public GameObject scrapPrefab; 

    private Transform targetTower;
    private TowerHealth towerHealth;

    void Awake()
    {
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

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
            }
        }
    }
    public void ScaleEnemyStats(float healthMultiplier, float speedMultiplier)
    {
        currentHealth = maxHealth * healthMultiplier;
        currentSpeed = baseSpeed * speedMultiplier;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
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
            Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            GameObject spawnedScrap = Instantiate(scrapPrefab, spawnPosition, Quaternion.identity);
            spawnedScrap.transform.parent = null; 
            spawnedScrap.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); // Forces it to exactly (1, 1, 1)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            if (towerHealth!=null)
            {
                towerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}