using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IPoolable
{

    public EnemyStats enemyStats;
    private float maxHealth;
    private float currentHealth;
    private float moveSpeed;
    public float expValue; 

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public GameObject DamageNumberPopup;
    private CinemachineImpulseSource impulseSource;
    public static event Action<Enemy> OnEnemyDied;


    public ParticleSystemPool particlePool;
    [SerializeField] private ParticleSystem enemyDeath;
    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyStats.sprite != null)
        {
            spriteRenderer.sprite = enemyStats.sprite;
        }
        originalColor = spriteRenderer.color;

        maxHealth = currentHealth = enemyStats.maxHealth;
        moveSpeed = enemyStats.moveSpeed;
        expValue = enemyStats.ExpValue; 

        
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
    public void Die()
    {
        impulseSource.GenerateImpulse(.2f);
        OnEnemyDied?.Invoke(this);

        Destroy(gameObject);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy Collided.");
        // TakeDamage(10);
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
