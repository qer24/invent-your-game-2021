using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyStrikeMod : Mod
{
    public float damageMultiplier = 0.5f;
    public float chargeTimeMultiplier = 2f;
    public float fireRateMultiplier = 2f;

    DamageModifier damageModifier;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        damageModifier = new DamageModifier(0, damageMultiplier);
        attachedWeapon.damageModifiers.Add(damageModifier);

        attachedWeapon.chargeTimeModifiers.Add(chargeTimeMultiplier);

        attachedWeapon.fireRateModifiers.Add(fireRateMultiplier);
    }

    public override void DetachWeapon()
    {
        attachedWeapon.damageModifiers.Remove(damageModifier);

        attachedWeapon.chargeTimeModifiers.Remove(chargeTimeMultiplier);

        attachedWeapon.fireRateModifiers.Remove(fireRateMultiplier);

        base.DetachWeapon();
    }
}
