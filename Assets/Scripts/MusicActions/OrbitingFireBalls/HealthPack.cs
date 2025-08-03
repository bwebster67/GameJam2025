using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public float healAmount = 25f;
    public GameObject healthPackPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone.");

            PlayerHealthController playerHealth = other.GetComponent<PlayerHealthController>();

            if (playerHealth != null)
            {
                playerHealth.IncreaseMaxHealth(10);
                Debug.Log("Healed player for " + 10);

                if (healthPackPrefab != null)
                {
                    Vector2 randomPos = new Vector2(
                        Random.Range(-15f, 15f),
                        Random.Range(-15f, 15f)
                    );
                    Instantiate(healthPackPrefab, randomPos, Quaternion.identity);
                    Debug.Log("Spawned new health pack at " + randomPos);
                }
                else
                {
                    Debug.LogWarning("HealthPackPrefab not assigned!");
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("No PlayerHealth component found on player.");
            }
        }
    }
}