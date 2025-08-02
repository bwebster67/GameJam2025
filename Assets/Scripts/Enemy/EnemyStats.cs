using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyStats : ScriptableObject
{

    public float maxHealth;
    public float moveSpeed;
    public Sprite sprite;
    public float ExpValue;

}