using UnityEngine;

public class CircleProjectile : ProjectileTemp
{

    /* projectile lifespan in seconds */
    public float projLifespan = 1f;

    void Start()
    {
        Destroy(gameObject, projLifespan);
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
