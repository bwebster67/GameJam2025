using System.Collections;
using UnityEngine;


[CreateAssetMenu(menuName = "MusicActions/MakePurpleVortexAction")]
public class MakeVortexAction : MusicAction 
{
    public GameObject vortexPrefab;
    public Vortex Vortex;

    public float damage;

   public override IEnumerator Execute(MonoBehaviour runner)
    {
        Vector2 playerPos = runner.transform.position;

        Transform target = FindNearestEnemy(playerPos);
   
        // First projectile towards target (or fallback)
        
        if (Vortex.TryGetComponent<Vortex>(out var vortexProj))
        {
            vortexProj.damage = damage;
            vortexProj.FireVortex(target);
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
