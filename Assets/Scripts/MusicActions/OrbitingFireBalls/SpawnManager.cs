using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistance = 12f;

    [Header("Spawn Weights (Total Should Sum to 1)")]
    [Range(0f, 1f)] public float topWeight = 0.25f;
    [Range(0f, 1f)] public float bottomWeight = 0.25f;
    [Range(0f, 1f)] public float leftWeight = 0.25f;
    [Range(0f, 1f)] public float rightWeight = 0.25f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 direction = GetWeightedDirection();
        Vector2 spawnPos = (Vector2)player.position + direction * spawnDistance;

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
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