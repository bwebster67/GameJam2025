using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public EnemyStats enemyStats;
    private float maxHealth;
    private float currentHealth;
    private float moveSpeed; 

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public GameObject DamageNumberPopup;
     private CinemachineImpulseSource impulseSource;
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

        
    }

    public void TakeDamage(float damage)
    {
        GameObject damagePopup = Instantiate(DamageNumberPopup, transform.position, Quaternion.identity);
        damagePopup.transform.GetChild(0).GetComponent<TextMesh>().text = $"{damage}"; 
        currentHealth -= damage;
        Debug.Log("Enemy health: " + currentHealth);
        StartCoroutine("FlashOnHit");

        // play screen shake
        impulseSource.GenerateImpulse(.12f); // can overload with float force. 1 is normal
        if (currentHealth <= 0)
        {
            impulseSource.GenerateImpulse(.24f);
            Destroy(gameObject);
        }
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

}
