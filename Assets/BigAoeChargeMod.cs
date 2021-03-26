using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAoeChargeMod : Mod
{
    public float damageMultiplierNormal = 1.5f;
    public float damageMultiplierOthers = 1.15f;
    public float chargeTimeMultiplier = 2.5f;
    public float aoeSizeMultiplier = 2f;

    DamageModifier damageModifier;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        if (attachedWeapon.isAoe && attachedWeapon.isCharged)
        {
            damageModifier = new DamageModifier(0, damageMultiplierNormal);
            attachedWeapon.damageModifiers.Add(damageModifier);

            attachedWeapon.chargeTimeModifiers.Add(chargeTimeMultiplier);

            attachedWeapon.sizeModifiers.Add(aoeSizeMultiplier);
        }
        else
        {
            damageModifier = new DamageModifier(0, damageMultiplierOthers);
            attachedWeapon.damageModifiers.Add(damageModifier);
        }
    }

    public override void DetachWeapon()
    {
        if (attachedWeapon.isAoe && attachedWeapon.isCharged)
        {
            attachedWeapon.chargeTimeModifiers.Remove(chargeTimeMultiplier);
            attachedWeapon.sizeModifiers.Remove(aoeSizeMultiplier);
        }
        attachedWeapon.damageModifiers.Remove(damageModifier);

        base.DetachWeapon();
    }
}
