using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<GameObject> enemyPrefabList;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnDistance = 15f;

    [Header("Spawn Weights (Total Should Sum to 1)")]
    [Range(0f, 1f)] public float topWeight = 0.25f;
    [Range(0f, 1f)] public float bottomWeight = 0.25f;
    [Range(0f, 1f)] public float leftWeight = 0.25f;
    [Range(0f, 1f)] public float rightWeight = 0.25f;

    private float spawnTimer;
    private float waveTimer;

    [Header("Wave Settings")]
    [SerializeField] private float waveInterval = 20f;
    private int baseWaveCount = 10;
    private int waveNumber = 0;

    void Update()
    {
        // Regular spawn (optional — keep if you want ambient spawns too)
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.05f); // Optional
        }

        // Wave spawn
        waveTimer += Time.deltaTime;
        if (waveTimer >= waveInterval)
        {
            spawnWave();
            waveTimer = 0f;
        }
    }

    void spawnWave()
    {
        waveNumber++;

        // Increase by +3, then +4, then +5...
        int extra = Mathf.Min(3 + (waveNumber - 1), 99); // cap if needed
        int enemyCount = baseWaveCount + (waveNumber - 1) * extra;

        Debug.Log($"Spawning Wave {waveNumber} with {enemyCount} enemies");

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector2 direction = GetWeightedDirection();
        Vector2 spawnPos = (Vector2)player.position + direction * spawnDistance;

        int randomIndex = Random.Range(0, enemyPrefabList.Count);
        GameObject randomEnemyPrefab = enemyPrefabList[randomIndex];

        Instantiate(randomEnemyPrefab, spawnPos, Quaternion.identity);
    }

    Vector2 GetWeightedDirection()
    {
        float rand = Random.value;

        if (rand < topWeight)
            return Vector2.up;
        else if (rand < topWeight + bottomWeight)
            return Vector2.down;
        else if (rand < topWeight + bottomWeight + leftWeight)
            return Vector2.left;
        else
            return Vector2.right;
    }
}
