using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorUpgrade : PlayerUpgrade
{
    public float armor = 2;
    PlayerDamagable playerDamagable;

    public override void Upgrade()
    {
        base.Upgrade();
        playerDamagable = playerController.GetComponent<PlayerDamagable>();
        playerDamagable.OnPlayerTakeDamage += ReduceDamageByArmour;
    }

    private void OnDestroy()
    {
        if (playerDamagable != null)
            playerDamagable.OnPlayerTakeDamage -= ReduceDamageByArmour;
    }

    void ReduceDamageByArmour(float damage)
    {
        playerDamagable.damageToTake = damage - armor;
    }
}
