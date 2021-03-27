using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasedAoeMod : Mod
{
    public float aoeMultiplier = 1.5f;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        attachedWeapon.sizeModifiers.Add(aoeMultiplier);
    }

    public override void DetachWeapon()
    {
        attachedWeapon.sizeModifiers.Remove(aoeMultiplier);

        base.DetachWeapon();
    }
}
