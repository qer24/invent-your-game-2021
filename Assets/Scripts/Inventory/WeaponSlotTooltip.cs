using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlotTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Weapon weapon;
    WeaponPickup pickup;
    [SerializeField] ScaleTween tooltip = null;
    [SerializeField] GameObject[] modSlots = null; 

    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI descriptionText = null;

    private void Start()
    {
        weapon = GetComponent<Weapon>();
        pickup = GetComponent<WeaponPickup>();

        weapon.OnTooltipUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        weapon.OnTooltipUpdate -= UpdateUI;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pickup.isInWorld && ModDrop.draggingMod) return;

        tooltip.gameObject.SetActive(true);
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
        string weaponName = $"{weapon.rarityString}{weapon.rodzajnikString}\n{weapon.name}";
        nameText.text = weaponName;
        descriptionText.text = weapon.description;
    }
}
