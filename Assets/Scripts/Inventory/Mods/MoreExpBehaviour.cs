using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreExpBehaviour : OnDamageBehaviour
{
    private void Start()
    {
        damager.OnDealDamage += AddExp;
    }

    private void OnDestroy()
    {
        damager.OnDealDamage -= AddExp;
    }

    void AddExp(float damage, Transform damagable)
    {
        if (damagable.TryGetComponent(out Enemy enemy))
        {
            if (enemy.health.currentHealth <= 0)
            {
                PlayerUpgradeManager.Instance.AddExp(enemy.expValue);
            }
            else
            {
                if (gameObject != null)
                    Destroy(gameObject);
            }
        }
    }
}
