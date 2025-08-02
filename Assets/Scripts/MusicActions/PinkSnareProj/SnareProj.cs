using UnityEditor.Toolbars;
using UnityEngine;

public class SnareProj : ProjectileTemp
{

    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] ScreenShake screenShake;

    private Vector2 direction;

    public void Fire(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;

        // Rotate to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);

        // Destroy after lifespan
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
       
        if (collider.CompareTag("Enemy"))
        {
            
            DamageEnemy(collider); 
            Destroy(gameObject);
        
        }
    }
}