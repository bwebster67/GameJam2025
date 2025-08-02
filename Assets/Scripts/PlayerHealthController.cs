using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthController : MonoBehaviour


{
    public TextMeshProUGUI healthBarText;
    [SerializeField] public bool isDead = false;
    [SerializeField] public float health = 100f;
    [SerializeField] public float maxHealth = 100f;
   
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem deathParticle2;
    [SerializeField] public Vector3 spawn;

    public HealthBar healthBar;
    public AudioClip playerDeathSFX;

    private void Start()
    {
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);

        transform.position = spawn;
       
     

    }
    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;
            healthBar.setHealth(health);


        }
    }
    public void Die()
    {
        if (playerDeathSFX != null) { SoundFXManager.Instance.PlaySoundClip(playerDeathSFX, transform, 1f, 1f); }
       
        isDead = true;
        health = 0;
        healthBar.setHealth(health);
        gameObject.SetActive(false);
        Debug.Log("Player Died!");
        Debug.LogWarning("Die() called on: " + gameObject.name);

        if (deathParticle != null) 
            {
            Instantiate(deathParticle, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            Instantiate(deathParticle2, transform.position, Quaternion.identity); }
    }
    public void Respawn(Vector2 spawnPoint = default(Vector2)) // <--- this code means paramater is optional and default vect2 = 0,0
    {
        gameObject.transform.position = spawnPoint;
        gameObject.SetActive(true);
        
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    
    }
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        health += amount; 
        healthBar.setMaxHealth(maxHealth);
        healthBar.setHealth(health);
      
    }

 
    void Update()
    {
        if (health <= 0)
        {
            Die();
            Debug.Log("Player lost all health");
        }
        if (Input.GetKey(KeyCode.Backspace))
        {
            TakeDamage(7.5f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
        
        }
    }
    
}
