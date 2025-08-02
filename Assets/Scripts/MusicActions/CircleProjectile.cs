using UnityEngine;

public class CircleProjectile : ProjectileTemp
{
    
     /* projectile lifespan in seconds */

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    protected override void DamageEnemy(Collider2D collider)
    {
        base.DamageEnemy(collider); 

       

        Debug.Log("CircleProjectile hit something");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collided");
        if (collider.gameObject.CompareTag("Enemy"))
        {
            DamageEnemy(collider);
        }
    }

}
