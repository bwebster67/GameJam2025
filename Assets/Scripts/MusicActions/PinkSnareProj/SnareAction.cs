using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "MusicActions/SnareAction")]
public class SnareAction : MusicAction
{
    [SerializeField] private GameObject snareProjPrefab;
    [SerializeField] private float damage = 7.5f;

    public override IEnumerator Execute(MonoBehaviour runner)
    {
        Vector2 playerPos = runner.transform.position;

        Transform target = FindNearestEnemy(playerPos);
        Vector2 targetDirection = target != null
            ? ((Vector2)target.position - playerPos).normalized
            : Random.insideUnitCircle.normalized;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector2 randomDirection2 = Random.insideUnitCircle.normalized;
        Vector2 randomDirection3 = Random.insideUnitCircle.normalized;

        // First projectile towards target (or fallback)
        GameObject snare1 = Object.Instantiate(snareProjPrefab, playerPos, Quaternion.identity);
        if (snare1.TryGetComponent<SnareProj>(out var snareProj1))
        {
            snareProj1.damage = damage;
            snareProj1.Fire(targetDirection);
        }

        // Second projectile towards random direction
        GameObject snare2 = Object.Instantiate(snareProjPrefab, playerPos, Quaternion.identity);
        if (snare2.TryGetComponent<SnareProj>(out var snareProj2))
        {
            snareProj2.damage = damage;
            snareProj2.Fire(randomDirection);
        }
        // Third projectile towards random direction
        GameObject snare3 = Object.Instantiate(snareProjPrefab, playerPos, Quaternion.identity);
        if (snare3.TryGetComponent<SnareProj>(out var snareProj3))
        {
            snareProj3.damage = damage;
            snareProj3.Fire(randomDirection2);
        }
        // Third projectile towards random direction
        GameObject snare4 = Object.Instantiate(snareProjPrefab, playerPos, Quaternion.identity);
        if (snare4.TryGetComponent<SnareProj>(out var snareProj4))
        {
            snareProj4.damage = damage;
            snareProj4.Fire(randomDirection3);
        }


        yield break;
    }

    private Transform FindNearestEnemy(Vector2 origin)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDistSqr = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distSqr = ((Vector2)enemy.transform.position - origin).sqrMagnitude;
            if (distSqr < minDistSqr)
            {
                minDistSqr = distSqr;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }
}
