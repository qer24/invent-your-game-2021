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

    protected float FinalHealth { get => enemyCard.maxHealth * (1 + 0.1f * DifficultyManager.Instance.currentDifficulty); }

    public virtual void Start()
    {

        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        screenConfiner = GetComponent<KeepOnScreen>();
        damagable = GetComponent<Damagable>();

        health.stats = enemyCard;
        health.currentHealth = FinalHealth;
        health.OnDeath += OnDeath;

        if (PlayerPersistencyMenager.Instance == null) return;
        player = PlayerPersistencyMenager.Instance.GetComponent<Rigidbody>();
        PlayerHealth.OnPlayerDeath += () => { if (this != null) enabled = false; };
        damagable.OnTakeDamage += OnDamageTaken;
    }

    private void OnDestroy()
    {
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
        EndScreen.EnemiesDestroyed++;
        Destroy(gameObject);
    }
}
