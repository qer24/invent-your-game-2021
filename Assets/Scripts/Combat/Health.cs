using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Stats stats;
    public float currentHealth = 0;

    public delegate void HealthAction(float currentHealthAmount);
    public HealthAction OnHealthChanged;
    public Action OnDeath;

    public float maxHealth;

    [HideInInspector] public bool isDead;

    public virtual void Awake()
    {
        maxHealth = stats.maxHealth;
        currentHealth = maxHealth;

        isDead = false;
    }

    public virtual void RemoveHealth(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0) Death();
    }

    public void RestoreHealth(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth);
    }

    protected void Death()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();
    }
}
