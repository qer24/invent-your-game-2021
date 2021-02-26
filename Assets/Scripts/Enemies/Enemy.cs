using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base script")]
    public Health health;
    public EnemyCard stats;

    protected static string playerTag = "Player";

    protected Rigidbody player;
    protected Rigidbody rb;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();

        health.OnDeath += OnDeath;
    }

    public virtual void OnDeath()
    {
        Destroy(gameObject);
    }
}
