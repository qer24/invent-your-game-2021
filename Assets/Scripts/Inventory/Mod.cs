using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mod : MonoBehaviour
{
    protected Weapon attachedWeapon;

    public virtual void AttachToWeapon(Weapon weapon)
    {
        attachedWeapon = weapon;
    }

    public virtual void DetachWeapon()
    {
        attachedWeapon = null;
    }
}
