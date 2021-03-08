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
        health.currentHealth = enemyCard.maxHealth;
        health.OnDeath += OnDeath;

        PlayerHealth.OnPlayerDeath += Disable;
    }

    void Disable()
    {
        enabled = false;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= Disable;
    }

    public virtual void OnDeath()
    {
        CinemachineShake.Instance.ShakeCamera(7, 0.4f);
        PlayerUpgradeManager.Instance.AddExp(expValue);
        Destroy(gameObject);
    }
}
