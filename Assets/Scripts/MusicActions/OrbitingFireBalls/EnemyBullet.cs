using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Player");

            PlayerHealthController health = collider.GetComponent<PlayerHealthController>();
            if (health != null)
            {
                health.TakeDamage(10f);
            }

            Destroy(gameObject);
        }
    }
}
