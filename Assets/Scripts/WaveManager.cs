using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData
    {
        public string name;
        public GameObject prefab;
        [Range(0, 100)] public float baseWeight; // Initial spawn probability weight
    }

    [Header("Enemy Prefabs")]
    public EnemyData blueEnemy;  // Easiest
    public EnemyData greenEnemy; // Medium
    public EnemyData redEnemy;   // Hardest

    [Header("Spawner Settings")]
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 10f;
    public float spawnEnemyInterval = 1.0f;

    [Header("Scaling Multipliers")]
    public float healthMultiplierPerWave = 0.15f; // +15% enemy health per wave
    public float speedMultiplierPerWave = 0.05f;  // +5% enemy speed per wave
    public int enemyNumbers = 4;

    public int waveNumber = 1;
    private int enemiesToSpawn;
    private float waveTimer;
    private bool isWaveActive = false;

    

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        waveTimer = 3f;
    }

    void Update()
    {
        activeEnemies.RemoveAll(item => item == null);

        if (isWaveActive)
        {
            if (activeEnemies.Count == 0)
            {
                EndWave();
            }
            return;
        }

        if (waveTimer <= 0)
        {
            StartCoroutine(SpawnWave());
        }
        else
        {
            waveTimer -= Time.deltaTime;
        }
    }

    System.Collections.IEnumerator SpawnWave()
    {
        isWaveActive = true;
        
        enemiesToSpawn = 10 + ((waveNumber-1)*enemyNumbers);
        if (enemiesToSpawn>75)
        {
            enemiesToSpawn = 75;
        }
        
        
        Debug.Log($"--- STARTING WAVE {waveNumber} ---");

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnEnemyInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        // 1. Calculate Dynamic Spawn Weights based on current Wave Number
        // Blue weight decreases, Green rises, Red rises even faster later
        float currentBlueWeight = Mathf.Max(10f, blueEnemy.baseWeight - (waveNumber * 5f));
        float currentGreenWeight = greenEnemy.baseWeight + (waveNumber * 3f);
        float currentRedWeight = redEnemy.baseWeight + (waveNumber * 6f);

        // Wave 1-2 lock safeguard: Make sure red doesn't accidentally show up instantly
        if (waveNumber < 3) currentRedWeight = 0;
        if (waveNumber < 2) currentGreenWeight = 0;

        float totalWeight = currentBlueWeight + currentGreenWeight + currentRedWeight;
        float randomValue = Random.Range(0f, totalWeight);

        GameObject selectedPrefab = blueEnemy.prefab;

        // 2. Select enemy based on weight calculation
        if (randomValue < currentBlueWeight)
        {
            selectedPrefab = blueEnemy.prefab;
        }
        else if (randomValue < currentBlueWeight + currentGreenWeight)
        {
            selectedPrefab = greenEnemy.prefab;
        }
        else
        {
            selectedPrefab = redEnemy.prefab;
        }

        // 3. Instantiate and apply stat modifiers
        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(selectedPrefab, randomPoint.position, randomPoint.rotation);
        activeEnemies.Add(enemy);

        // 4. Pass stat scaling directly to the enemy AI script
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            // Base stats are scaled dynamically by the wave number
            // Equation: Stat * (1 + (Wave * Multiplier))
            float healthModifier = 1f + (waveNumber * healthMultiplierPerWave);
            float speedModifier = 1f + (waveNumber * speedMultiplierPerWave);
            
            enemyAI.ScaleEnemyStats(healthModifier, speedModifier);
        }
    }

    void EndWave()
    {
        isWaveActive = false;
        waveNumber++;
        waveTimer = timeBetweenWaves;
    }
}