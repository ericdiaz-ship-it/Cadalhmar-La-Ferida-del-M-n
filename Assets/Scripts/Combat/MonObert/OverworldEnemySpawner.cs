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
        Vector2 randomPos = Vector2.zero;
        bool validPositionFound = false;

        // Intentem trobar una posició lliure de parets/obstacles (màxim 15 intents)
        for (int i = 0; i < 15; i++)
        {
            randomPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;

            // Comprovem si hi ha algun Collider (parets, aigua) en aquest punt (radi de 0.5 unitats)
            Collider2D hitCollider = Physics2D.OverlapCircle(randomPos, 0.5f);
            
            // Si no toquem res, o si el que toquem és només un trigger (com una zona d'esdeveniment), la posició és vàlida
            if (hitCollider == null || hitCollider.isTrigger)
            {
                validPositionFound = true;
                break;
            }
        }

        // Si no trobem lloc lliure, cancel·lem l'spawn aquest cop
        if (!validPositionFound)
        {
            return;
        }

        // Instancia l'enemic a la posició vàlida
        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);

        // Assigna dades aleatòries de criatura
        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI != null && possibleEnemies.Length > 0)
        {
            CreatureProfile randomProfile = possibleEnemies[Random.Range(0, possibleEnemies.Length)];
            enemyAI.creatureData = randomProfile.GenerateDataForLevel(Random.Range(1, 6)); // Nivell aleatori de 1 a 5
        }

        spawnedEnemies.Add(enemy);
    }
}