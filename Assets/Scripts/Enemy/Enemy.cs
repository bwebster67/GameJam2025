using System.Collections;
using System.Collections.Generic;
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

        // TakeDamage(10);
    }

    public void TakeDamage(float damage)
    {
        GameObject damagePopup = Instantiate(DamageNumberPopup, transform.position, Quaternion.identity);
        damagePopup.transform.GetChild(0).GetComponent<TextMesh>().text = $"{damage}"; 
        currentHealth -= damage;
        Debug.Log("Enemy health: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy Collided.");
        // TakeDamage(10);
    }

    // IEnumerator FlashOnHit()
    // {
    //     yield return new WaitForSeconds(0.05f);
    //     spriteRenderer.color = Color.white;
    //     yield return new WaitForSeconds(0.05f);
    //     spriteRenderer.color = originalColor;
    // }

}
