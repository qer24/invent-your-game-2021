using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreHpUpgrade : PlayerUpgrade
{
    public override void Upgrade()
    {
        base.Upgrade();
        var health = playerController.GetComponent<PlayerHealth>();
        health.maxHealth *= 1.2f;
        health.RestoreHealth(health.maxHealth);

        LeanTween.scaleX(health.healthSlider.gameObject, health.healthSlider.transform.localScale.x * 1.2f, 0.5f).setEase(LeanTweenType.easeOutBack);
    }
}
