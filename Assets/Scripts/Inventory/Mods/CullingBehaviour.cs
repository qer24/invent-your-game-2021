using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingBehaviour : OnDamageBehaviour
{
    private void Start()
    {
        damager.OnDealDamage += Cull;
    }

    private void OnDestroy()
    {
        if(damager != null)
        {
            damager.OnDealDamage -= Cull;
        }
    }

    private static void Cull(float damage, Transform damagable)
    {
        if (damagable.TryGetComponent(out Enemy enemy))
        {
            if (enemy.health.currentHealth < enemy.FinalHealth * 0.1f)
            {
                enemy.GetComponent<Damagable>().TakeDamage(enemy.health.currentHealth + 1);
            }
        }
    }
}
