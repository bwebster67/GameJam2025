using UnityEngine;

public class Vortex : ProjectileTemp
{
    [SerializeField] ScreenShake screenShake;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("vortex");
            DamageEnemy(collider);
           
        }
    }

    private void Start()
    {
        Destroy(gameObject, lifespan);
    }
}
