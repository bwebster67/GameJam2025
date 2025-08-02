using System.Collections;
using UnityEngine;


[CreateAssetMenu(menuName = "MusicActions/MakePurpleVortexAction")]
public class MakeVortexAction : MusicAction 
{
    public GameObject vortexPrefab;
    public Vortex Vortex;
    private int vortexRange = 4;

    public float damage;

    public override IEnumerator Execute(MonoBehaviour runner)
    {
        Vector2 playerPos = runner.transform.position;
        Transform target = FindNearestEnemy(playerPos);

        Vector2 spawnPos;
        if (target != null)
        {
            Vector2 targetPos = target.position;
            Vector2 dir = (targetPos - playerPos).normalized;
            float distance = Vector2.Distance(playerPos, targetPos);

            if (distance <= vortexRange)
                spawnPos = targetPos;
            else
                spawnPos = playerPos + dir * vortexRange;
        }
        else
        {
            spawnPos = playerPos; // fallback if no enemies
        }

        GameObject vortex = Object.Instantiate(vortexPrefab, spawnPos, Quaternion.identity);
        if (vortex.TryGetComponent<Vortex>(out var vortexProj))
        {
            vortexProj.damage = damage;
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
