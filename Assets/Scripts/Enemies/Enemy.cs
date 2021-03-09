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
    protected Damagable damagable;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        screenConfiner = GetComponent<KeepOnScreen>();
        damagable = GetComponent<Damagable>();

        health.stats = enemyCard;
        health.currentHealth = enemyCard.maxHealth;
        health.OnDeath += OnDeath;

        PlayerHealth.OnPlayerDeath += Disable;
        damagable.OnTakeDamage += OnDamageTaken;
    }

    void Disable()
    {
        enabled = false;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDeath -= Disable;
        if(damagable != null)
            damagable.OnTakeDamage -= OnDamageTaken;
    }

    public virtual void OnDamageTaken()
    {
        if (!string.IsNullOrEmpty(enemyCard.onTakeDamageAudio))
        {
            AudioManager.Play(enemyCard.onTakeDamageAudio, true);
        }
    }

    public virtual void OnDeath()
    {
        if(!string.IsNullOrEmpty(enemyCard.onDeathAudio))
        {
            AudioManager.Play(enemyCard.onDeathAudio, true);
        }

        CinemachineShake.Instance.ShakeCamera(7, 0.4f);
        PlayerUpgradeManager.Instance.AddExp(expValue);
        Destroy(gameObject);
    }
}
