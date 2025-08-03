using TMPro;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public TextMeshProUGUI healthBarText;
    [SerializeField] public bool isDead = false;
    [SerializeField] public float health = 100f;
    [SerializeField] public float maxHealth = 100f;

    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem deathParticle2;
    [SerializeField] public Vector3 spawn;
    public UIManagerScript UIManagerScript;
    public HealthBar healthBar;
    public AudioClip playerDeathSFX;
    [SerializeField] PlayerMusicController playerMusicController;

    private void Start()
    {
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);

        // Automatically find UIManagerScript if not assigned
        if (UIManagerScript == null)
        {
            UIManagerScript = FindObjectOfType<UIManagerScript>();
            if (UIManagerScript == null)
                Debug.LogError("UIManagerScript not found in scene!");
        }
        if (playerMusicController == null)
        {
            playerMusicController = FindObjectOfType<PlayerMusicController>();
            if (playerMusicController == null)
                Debug.LogError("playermusic controller not found");
        }

        transform.position = spawn;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;
        }
    }

    public void Die()
    {

        if (UIManagerScript != null)
        {
            UIManagerScript.openRespawnMenu();
        }
        else
        {
            Debug.LogWarning("UIManagerScript is not assigned!");
        }

        if (playerDeathSFX != null)
        {
            SoundFXManager.Instance.PlaySoundClip(playerDeathSFX, transform, 1f, 1f);
        }

        isDead = true;
        health = 0;
        healthBar.setHealth(health);
        gameObject.SetActive(false);

        Debug.Log("Player Died!");
        Debug.LogWarning("Die() called on: " + gameObject.name);

        if (deathParticle != null)
        {
            Instantiate(deathParticle, transform.position, Quaternion.Euler(-90f, 0f, 0f));
        }
        if (deathParticle2 != null)
        {
            Instantiate(deathParticle2, transform.position, Quaternion.identity);
        }

        Time.timeScale = 0;
    }

    public void Respawn(Vector2 spawnPoint = default(Vector2))
    {
        gameObject.transform.position = spawnPoint;
        gameObject.SetActive(true);

        health = maxHealth;
        isDead = false;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(maxHealth);
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        health += amount;
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(maxHealth);
    }

    void Update()
    {
        if (health <= 0 && !isDead)
        {
            Die();
            Debug.Log("Player lost all health");
        }

        if (Input.GetKey(KeyCode.Backspace))
        {
            TakeDamage(7.5f);
        }
    }
}
