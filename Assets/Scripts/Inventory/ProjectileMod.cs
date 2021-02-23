using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMod : Mod
{
    public UnityEngine.Object projectileBehaviour;

    public float damageMultiplierForProjectile = 1f;
    public float addedDamageForProjectile = 0f;
    public float damageMultiplierForNonProjectile = 1f;
    public float addedDamageForNonProjectile = 0f;

    DamageModifier damageModifier;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        if (attachedWeapon.isProjectile)
        {
            damageModifier = new DamageModifier(addedDamageForProjectile, damageMultiplierForProjectile);
            attachedWeapon.damageModifiers.Add(damageModifier);
        }
        else
        {
            damageModifier = new DamageModifier(addedDamageForNonProjectile, damageMultiplierForNonProjectile);
            attachedWeapon.damageModifiers.Add(damageModifier);
        }

        attachedWeapon.OnProjectileCreated += AddBehaviour;
    }


    public override void DetachWeapon()
    {
        if (attachedWeapon == null) return;

        attachedWeapon.damageModifiers.Remove(damageModifier);
        attachedWeapon.OnProjectileCreated -= AddBehaviour;

        base.DetachWeapon();
    }

    void AddBehaviour(GameObject projectile)
    {
        projectile.AddComponent(Type.GetType(projectileBehaviour.name));
    }
}
