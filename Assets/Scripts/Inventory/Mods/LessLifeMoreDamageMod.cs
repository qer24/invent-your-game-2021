using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LessLifeMoreDamageMod : Mod
{
    PlayerHealth playerHealth;

    DamageModifier damageModifier;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        playerHealth = GetComponentInParent<PlayerHealth>();
        damageModifier = new DamageModifier(0, 1.4f);

        playerHealth.OnHealthChanged += SetMultiplier;
    }

    void SetMultiplier(float currentHealth)
    {
        if(currentHealth / playerHealth.stats.maxHealth > 0.25f)
        {
            if (attachedWeapon.damageModifiers.Contains(damageModifier))
            {
                attachedWeapon.damageModifiers.Remove(damageModifier);
                attachedWeapon.StartCoroutine(attachedWeapon.ForceReloadTooltip());
            }
        }
        else
        {
            if(!attachedWeapon.damageModifiers.Contains(damageModifier))
            {
                attachedWeapon.damageModifiers.Add(damageModifier);
                attachedWeapon.StartCoroutine(attachedWeapon.ForceReloadTooltip());
            }
        }
    }
}
