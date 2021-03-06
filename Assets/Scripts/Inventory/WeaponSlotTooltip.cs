using ProcGen;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;

public class WeaponSlotTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Weapon weapon;
    WeaponPickup pickup;
    [SerializeField] ScaleTween tooltip = null;
    [SerializeField] GameObject[] modSlots = null; 

    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI descriptionText = null;

    LocalizeStringEvent[] localizedStrings;

    private void Start()
    {
        weapon = GetComponent<Weapon>();
        pickup = GetComponent<WeaponPickup>();

        localizedStrings = GetComponents<LocalizeStringEvent>();

        weapon.OnTooltipUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        if(weapon != null)
            weapon.OnTooltipUpdate -= UpdateUI;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Room.enemiesAlive.Count > 0) return;
        if (pickup.isInWorld && ModDrop.DraggingMod) return;

        AudioManager.Play("event:/SFX/UI/UIHover", true);
        StartCoroutine(ForceReloadTooltip());
        UpdateUI();
        tooltip.gameObject.SetActive(true);
    }

    IEnumerator ForceReloadTooltip()
    {
        yield return null;

        foreach (var str in localizedStrings)
        {
            str.RefreshString();
        }

        yield return null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        if (weapon.modSlots > 8 || weapon.modSlots < 0)
        {
            Debug.LogError("Invalid mod slot count");
        }

        for (int i = 0; i < modSlots.Length; i++)
        {
            modSlots[i].SetActive(i <= (weapon.modSlots - 1) ? true : false);
        }
        string weaponName = $"{weapon.rarityString}{weapon.rodzajnikString}</color>\n{weapon.name}";
        nameText.text = weaponName;
        descriptionText.text = weapon.description;
    }
}
