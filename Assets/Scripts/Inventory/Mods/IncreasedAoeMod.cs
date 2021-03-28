using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasedAoeMod : Mod
{
    public float aoeMultiplier = 1.5f;
    public float nonAoeDamageMultiplier = 1.05f;

    DamageModifier damageModifier;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        if(attachedWeapon.isAoe)
        {
            attachedWeapon.sizeModifiers.Add(aoeMultiplier);
        }else
        {
            damageModifier = new DamageModifier(0, nonAoeDamageMultiplier);
            attachedWeapon.damageModifiers.Add(damageModifier);
        }
    }

    public override void DetachWeapon()
    {
        if (attachedWeapon.isAoe)
        {
            attachedWeapon.sizeModifiers.Remove(aoeMultiplier);
        }
        else
        {
            attachedWeapon.damageModifiers.Remove(damageModifier);
        }

        base.DetachWeapon();
    }
}
