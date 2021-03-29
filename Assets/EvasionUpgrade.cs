using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasionUpgrade : PlayerUpgrade
{
    PlayerDamagable playerDamagable;

    public override void Upgrade()
    {
        base.Upgrade();
        playerDamagable = playerController.GetComponent<PlayerDamagable>();
        playerDamagable.OnPlayerTakeDamage += TryEvade;
    }

    private void OnDestroy()
    {
        if(playerDamagable != null)
            playerDamagable.OnPlayerTakeDamage -= TryEvade;
    }

    void TryEvade(float damage)
    {
        if(Random.value < 0.35f)
        {
            playerDamagable.damageToTake = damage * 2f;
        }else
        {
            playerDamagable.damageToTake = 0;
        }
    }
}
