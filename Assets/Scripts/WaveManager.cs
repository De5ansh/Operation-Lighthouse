using UnityEngine;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyData
    {
        public string name;
        public GameObject prefab;
        [Range(0, 100)] public float baseWeight; 
    }

    [Header("Enemy Prefabs")]
    public EnemyData blueEnemy;  
    public EnemyData greenEnemy; 
    public EnemyData redEnemy;   

    [Header("Spawner Settings")]
    public Transform[] spawnPoints;
    public float timeBetweenWaves = 10f;
    public float spawnEnemyInterval = 1.0f;

    [Header("Scaling Multipliers")]
    public float healthMultiplierPerWave = 0.15f; 
    public float speedMultiplierPerWave = 0.05f;  
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

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnEnemyInterval);
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        float currentBlueWeight = Mathf.Max(10f, blueEnemy.baseWeight - (waveNumber * 5f));
        float currentGreenWeight = greenEnemy.baseWeight + (waveNumber * 3f);
        float currentRedWeight = redEnemy.baseWeight + (waveNumber * 6f);

        if (waveNumber < 3) currentRedWeight = 0;
        if (waveNumber < 2) currentGreenWeight = 0;

        float totalWeight = currentBlueWeight + currentGreenWeight + currentRedWeight;
        float randomValue = Random.Range(0f, totalWeight);

        GameObject selectedPrefab = blueEnemy.prefab;

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

        Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(selectedPrefab, randomPoint.position, randomPoint.rotation);
        activeEnemies.Add(enemy);

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
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