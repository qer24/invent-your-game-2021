using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenUpgrade : PlayerUpgrade
{
    PlayerHealth health;

    public override void Upgrade()
    {
        base.Upgrade();
        health = playerController.GetComponent<PlayerHealth>();
        InvokeRepeating(nameof(Heal), 1f, 1f);
    }

    void Heal()
    {
        health.RestoreHealth(1);
    }
}
