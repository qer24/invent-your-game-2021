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

        StartCoroutine(UpdateWeaponTooltip());
    }

    IEnumerator UpdateWeaponTooltip()
    {
        //hack to refresh the localised string database
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        yield return null;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];

        attachedWeapon.OnTooltipUpdate?.Invoke();
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
