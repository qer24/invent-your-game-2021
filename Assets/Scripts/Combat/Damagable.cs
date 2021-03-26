using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public interface IDamagable
{
    void TakeDamage(float amount);
}

public class Damagable : MonoBehaviour, IDamagable
{
    public Health hp;
    public DamagePopup damagePopup;

    public Action OnTakeDamage;

    public virtual void TakeDamage(float amount)
    {
        if (hp.isDead) return;
        if (!enabled) return;
        if (amount <= 0) return;

        OnTakeDamage?.Invoke();
        hp.RemoveHealth(amount);
        EndScreen.DamageDealt += (int)amount;

        Lean.Pool.LeanPool.Spawn(
            damagePopup,
            transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 2.5f, Random.Range(-0.5f, 0.5f)),
            damagePopup.transform.rotation
            ).Init((int)amount);
    }
}
