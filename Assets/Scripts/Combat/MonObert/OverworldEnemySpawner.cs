using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldEnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public int maxEnemies = 5;
    public float spawnRadius = 10f;
    public float spawnInterval = 5f;

    public CreatureProfile[] possibleEnemies; // Array of possible enemy profiles

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private float spawnTimer;

    void Start()
    {
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 && spawnedEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Random position within radius
        Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

        // Instantiate enemy
        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        // Assign random creature data
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null && possibleEnemies.Length > 0)
        {
            CreatureProfile randomProfile = possibleEnemies[Random.Range(0, possibleEnemies.Length)];
            enemyAI.creatureData = randomProfile.GenerateDataForLevel(Random.Range(1, 6)); // Random level 1-5
        }

        spawnedEnemies.Add(enemy);
    }
}