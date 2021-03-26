using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public abstract class Mod : MonoBehaviour
{
    protected Weapon attachedWeapon;

    public virtual void AttachWeapon(Weapon weapon)
    {
        attachedWeapon = weapon;

        attachedWeapon.StartCoroutine(attachedWeapon.ForceReloadTooltip());
    }

    private void OnDestroy()
    {
        if (attachedWeapon == null) return;

        DetachWeapon();
    }

    public virtual void DetachWeapon()
    {
        attachedWeapon = null;
    }
}
