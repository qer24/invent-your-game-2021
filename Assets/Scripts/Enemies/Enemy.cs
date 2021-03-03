using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Base script")]
    protected Health health;
    public EnemyCard enemyCard;
    [HideInInspector] public int expValue = 1;

    protected static string playerTag = "Player";

    protected Rigidbody player;
    protected Rigidbody rb;
    protected KeepOnScreen screenConfiner;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        screenConfiner = GetComponent<KeepOnScreen>();

        health.stats = enemyCard;
        health.OnDeath += OnDeath;
    }

    public virtual void OnDeath()
    {
        PlayerUpgradeManager.Instance.AddExp(expValue);
        Destroy(gameObject);
    }
}
