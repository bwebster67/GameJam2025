using System.Collections;
using UnityEngine;


[CreateAssetMenu(menuName = "MusicActions/MakeFireBallAction")]
public class MakeFireBallAction : MusicAction 
{
    public GameObject fireball;
 

    public float damage;

   public override IEnumerator Execute(MonoBehaviour runner)
    {
        Vector2 playerPos = runner.transform.position;

        Transform target = FindNearestEnemy(playerPos);
        Vector2 targetDirection = target != null
            ? ((Vector2)target.position - playerPos).normalized
            : Random.insideUnitCircle.normalized;

        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // First projectile towards target (or fallback)
        GameObject _Fireball = Object.Instantiate(fireball, playerPos, Quaternion.identity);
        if (_Fireball.TryGetComponent<FireBall>(out var fireballProj))
        {
            fireballProj.damage = damage;
            fireballProj.Fire(targetDirection);
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
