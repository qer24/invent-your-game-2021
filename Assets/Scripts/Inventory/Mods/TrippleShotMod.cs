using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippleShotMod : Mod
{
    public float angle = 10;
    public float damageMultiplierForProjectile = 0.6f;
    public float damageMultiplierForNonProjectile = 1.25f;

    DamageModifier damageModifier;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        if(attachedWeapon.isProjectile)
        {
            damageModifier = new DamageModifier(0, damageMultiplierForProjectile);
            attachedWeapon.damageModifiers.Add(damageModifier);
            attachedWeapon.OnProjectileAttack += ShootAdditionalBullets;
        }else
        {
            damageModifier = new DamageModifier(0, damageMultiplierForNonProjectile);
            attachedWeapon.damageModifiers.Add(damageModifier);
        }
    }

    public override void DetachWeapon()
    {
        if (attachedWeapon.isProjectile)
        {
            attachedWeapon.OnProjectileAttack -= ShootAdditionalBullets;
        }
        attachedWeapon.damageModifiers.Remove(damageModifier);

        base.DetachWeapon();
    }

    void ShootAdditionalBullets(Vector3 position, Quaternion rotation, string enemyTag, Material projectileMaterial)
    {
        attachedWeapon.ShootProjectile(position, rotation * Quaternion.AngleAxis(angle, Vector3.up), enemyTag, projectileMaterial);
        attachedWeapon.ShootProjectile(position, rotation * Quaternion.AngleAxis(-angle, Vector3.up), enemyTag, projectileMaterial);
    }
}
