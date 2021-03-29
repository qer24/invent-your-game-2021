using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleEdgedUpgrade : PlayerUpgrade
{
    public float mutliplier = 0.85f;
    PlayerDamagable playerDamagable;
    PlayerHealth health;

    public override void Upgrade()
    {
        base.Upgrade();
        playerDamagable = playerController.GetComponent<PlayerDamagable>();
        playerDamagable.OnPlayerTakeDamage += ReduceDamage;

        health = playerController.GetComponent<PlayerHealth>();
        InvokeRepeating(nameof(DealDamage), 2f, 2f);
    }

    private void OnDestroy()
    {
        if (playerDamagable != null)
            playerDamagable.OnPlayerTakeDamage -= ReduceDamage;
    }

    void ReduceDamage(float damage)
    {
        playerDamagable.damageToTake = damage * mutliplier;
    }

    void DealDamage()
    {
        if (ProcGen.Room.enemiesAlive == null) return;
        if (ProcGen.Room.enemiesAlive.Count <= 0) return;

        health.RemoveHealth(1);
    }
}
