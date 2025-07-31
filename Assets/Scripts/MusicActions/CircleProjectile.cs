using UnityEngine;

public class CircleProjectile : MonoBehaviour
{
    public float damage;
    public float lifespan = 3f; /* projectile lifespan in seconds */

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    void DamageEnemy(Collider2D collider)
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
