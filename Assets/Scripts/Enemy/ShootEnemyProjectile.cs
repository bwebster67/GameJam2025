using UnityEngine;

public class ShootEnemyProjectile : MonoBehaviour
{
    private Transform player;
    public float speed = 5;
    private Vector2 moveDirection;
    public float projLifespan = 3f;
    public PlayerHealthController playerHealthController;
    public float damage;



    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        playerHealthController = playerObject.GetComponent<PlayerHealthController>();
        if (playerObject != null)
        {
            player = playerObject.transform;
            // Initial direction towards player
            moveDirection = (player.position - transform.position).normalized;
        }
        Destroy(gameObject, projLifespan); // destroy after snair seconds desu
    }

    void Update()
    {
        if (player == null) return;

        // Calculate desired direction towards player
        Vector2 desiredDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;

        // Gently curve: interpolate between current direction and desired direction
        float curveStrength = 0.01f; // Lower = gentler curve
        moveDirection = Vector2.Lerp(moveDirection, desiredDirection, curveStrength).normalized;

        // Move projectile
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealthController.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
