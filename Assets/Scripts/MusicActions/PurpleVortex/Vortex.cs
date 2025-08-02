using UnityEditor.Toolbars;
using UnityEngine;

public class Vortex : ProjectileTemp
{

  
    [SerializeField] ScreenShake screenShake;
    

    private Vector2 direction;

    [SerializeField] private int vortexRange = 7;
    public void FireVortex(Transform target) {
        Vector2 origin = transform.position;
        Vector2 targetPos = target.position;
        Vector2 direction = (targetPos - origin).normalized;

        float distance = Vector2.Distance(origin, targetPos);
        Vector2 spawnPos;

        if (distance <= vortexRange)
        {
            spawnPos = targetPos; // Spawn directly on the target
        }
        else
        {
            spawnPos = origin + direction * vortexRange; // Spawn at edge of range toward target
        }

        GameObject vortex = Instantiate(gameObject, spawnPos, Quaternion.identity);
        Destroy(vortex, lifespan); // Destroy vortex after lifespan

      

 
    }

    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("vortex");
            DamageEnemy(collider);
            Vector2 position2D = transform.position;  //translates vector2d to position
        
            Destroy(gameObject);

          
        }
    }
}