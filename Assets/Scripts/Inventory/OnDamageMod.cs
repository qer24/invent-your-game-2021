using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamageMod : Mod
{
    public GameObject behaviourPrefab;

    public override void AttachWeapon(Weapon weapon)
    {
        base.AttachWeapon(weapon);

        attachedWeapon.OnDamageBehaviours.Add(behaviourPrefab);
    }

    public override void DetachWeapon()
    {
        if(attachedWeapon.OnDamageBehaviours.Contains(behaviourPrefab))
            attachedWeapon.OnDamageBehaviours.Remove(behaviourPrefab);

        base.DetachWeapon();
    }

    private void OnDestroy()
    {
        if(attachedWeapon != null)
        {
            if(attachedWeapon.OnDamageBehaviours.Contains(behaviourPrefab))
                attachedWeapon.OnDamageBehaviours.Remove(behaviourPrefab);
        }
    }
}
