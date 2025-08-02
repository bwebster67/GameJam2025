using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int initialSize = 70;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    public GameObject GetFromPool()
    {
        GameObject enemy;

        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else
        {
            enemy = Instantiate(enemyPrefab);
        }

        enemy.SetActive(true);

        if (enemy.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnSpawnFromPool();
        }

        return enemy;
    }

    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        pool.Enqueue(enemy);
    }
}
