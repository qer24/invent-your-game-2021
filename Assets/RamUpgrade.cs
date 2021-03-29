using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamUpgrade : PlayerUpgrade
{
    public float baseDamage = 20f;
    public float velocityMultiplier = 0.05f;
    public float minVelocity = 5f;

    public override void Upgrade()
    {
        base.Upgrade();
        playerController.OnKnockback += DealRamDamage;
    }

    private void OnDestroy()
    {
        playerController.OnKnockback -= DealRamDamage;
    }

    void DealRamDamage(Transform colidee, float playerVelocity)
    {
        if (!colidee.CompareTag("Enemy")) return;
        if (playerVelocity < minVelocity) return;
        if(colidee.TryGetComponent<IDamagable>(out var damagable))
        {
            float damage = baseDamage * (1f + playerVelocity * velocityMultiplier) * (1 + 0.1f * (DifficultyManager.Instance.currentDifficulty - 1));
            damagable.TakeDamage(damage);
        }
    }
}
