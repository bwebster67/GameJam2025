using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;

public class ShootEnemyMovement : MonoBehaviour
{

    private GameObject player;
    private Enemy enemy;
    public float attackDelay = 5f;
    public float distance;
    public bool isAttacking;

    public GameObject ProjectilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }

        enemy = GetComponent<Enemy>();

    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        if (distance > 6)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, enemy.enemyStats.moveSpeed * Time.deltaTime);
        }
        if (!isAttacking)
        {
            StartCoroutine(AttackWithDelay());
        }
    }

    public IEnumerator AttackWithDelay()
    {
        isAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        Attack();
        isAttacking = false;
    }

    public void Attack()
    {
        GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
        ShootEnemyProjectile shootEnemyProjectile = projectile.GetComponent<ShootEnemyProjectile>();
        shootEnemyProjectile.damage = enemy.enemyStats.damage;
    }
}