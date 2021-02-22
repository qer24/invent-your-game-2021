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
        var currentLocale = LocalizationSettings.SelectedLocale;
        //hack to refresh the localised string database
        if (currentLocale.Identifier == "en")
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        else
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];

        yield return null;
        LocalizationSettings.SelectedLocale = currentLocale;

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
