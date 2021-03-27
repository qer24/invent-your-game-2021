using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinsStrikeMod : Mod
{
    public float damageMultiplier;
    public float velocityThreshold = 3f;
    PlayerController playerController;

    DamageModifier damageModifier;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        playerController = GetComponentInParent<PlayerController>();
        damageModifier = new DamageModifier(0, damageMultiplier);

        playerController.OnVelocityChange += SetMultiplier;
    }

    void SetMultiplier(float currentVelocity)
    {
        if (attachedWeapon == null) return;

        if (currentVelocity < velocityThreshold)
        {
            if (!attachedWeapon.damageModifiers.Contains(damageModifier))
            {
                attachedWeapon.damageModifiers.Add(damageModifier);
                attachedWeapon.StartCoroutine(attachedWeapon.ForceReloadTooltip());
            }
        }
        else
        {
            if (attachedWeapon.damageModifiers.Contains(damageModifier))
            {
                attachedWeapon.damageModifiers.Remove(damageModifier);
                attachedWeapon.StartCoroutine(attachedWeapon.ForceReloadTooltip());
            }
        }
    }
}
