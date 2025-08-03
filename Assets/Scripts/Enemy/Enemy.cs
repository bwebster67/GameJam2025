using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IPoolable
{

    public EnemyStats enemyStats;
    private float maxHealth;
    private float currentHealth;
    public float expValue;


    private Color originalColor = new Color(109f/255f, 73f/255f, 109f/255f, 1f);
    public GameObject DamageNumberPopup;
    public ParticleSystemPool particlePool;
    [SerializeField] private ParticleSystem enemyDeath;

    private SpriteRenderer spriteRenderer;
    private CinemachineImpulseSource impulseSource;
    public static event Action<Enemy> OnEnemyDied;
    public GameObject player;
    public PlayerHealthController playerHealthController;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerHealthController = player.GetComponent<PlayerHealthController>();
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyStats.sprite != null)
        {
            spriteRenderer.sprite = enemyStats.sprite;
        }
        maxHealth = currentHealth = enemyStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        GameObject damagePopup = Instantiate(DamageNumberPopup, transform.position, Quaternion.identity);
        damagePopup.transform.GetChild(0).GetComponent<TextMesh>().text = $"{damage}";
        currentHealth -= damage;
        Debug.Log("Enemy health: " + currentHealth);
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(FlashOnHit());
        }

        // play screen shake
        impulseSource.GenerateImpulse(.12f); // can overload with float force. 1 is normal
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealthController.TakeDamage(enemyStats.damage);
            Debug.Log($"Enemy collided with player dealing {enemyStats.damage} damage.");
        }
    }
    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (impulseSource == null)
        {
            Debug.LogWarning("No impulse source found on enemy.");
        }
    }
    IEnumerator FlashOnHit()
    {
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = originalColor;
    }
    public void OnSpawnFromPool()
    {
        // This is called every time the enemy is re-used
        currentHealth = maxHealth; 
        Debug.Log("Enemy reset");
    }

    private void Die()
    {
        OnEnemyDied.Invoke(this);
        /// Particle Stuff--------
        ParticleSystem ps = particlePool.Get();
        ps.transform.position = transform.position;
        ps.Play();
        StartCoroutine(HandleDeathWithParticle(ps, ps.main.duration));

        ///---------------------------------


        impulseSource.GenerateImpulse(.24f);
        Transform psTrans = enemyDeath.transform;    ///spawn death particles
        Transform enemyTrans = gameObject.transform;
        psTrans.position = enemyTrans.position;
        enemyDeath.Play();

        gameObject.SetActive(false);
    }
    private IEnumerator HandleDeathWithParticle(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);

       
        particlePool.Return(ps);

        // THEN deactivate the enemy
        gameObject.SetActive(false); 
    }
    //////////////////////////////

    // Pooling Particles

    private IEnumerator ReturnParticleAfterDelay(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        particlePool.Return(ps);
    }

    //////////////////////
    ///
    /// To make Object pooling more efficient, we need to disable costly features on enemies when they're disabled.
    ///
    //////////////////////

    private void OnDisable()
    {
        
    }

}
