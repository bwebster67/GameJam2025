using UnityEngine;

public abstract class ProjectileTemp : MonoBehaviour
{
    public float damage;
    public float lifespan = 3f; /* projectile lifespan in seconds 
                               -- if we want to change lifespan in subclass, just do it in OnEnable()*/
                                

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    protected virtual void DamageEnemy(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }
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
