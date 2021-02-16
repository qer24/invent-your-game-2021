using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlotTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] ScaleTween tooltip = null;
    [SerializeField] GameObject[] modSlots = null; 

    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI descriptionText = null;

    private void OnEnable()
    {
        UpdateUI();
        currentWeapon.OnTooltipUpdate += UpdateUI;
    }

    private void OnDisable()
    {
        currentWeapon.OnTooltipUpdate -= UpdateUI;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }

    void UpdateUI()
    {
        if (currentWeapon.modSlots > 8 || currentWeapon.modSlots < 0)
        {
            Debug.LogError("Invalid mod slot count");
        }

        for (int i = 0; i < modSlots.Length; i++)
        {
            modSlots[i].SetActive(i <= (currentWeapon.modSlots - 1) ? true : false);
        }
        nameText.text = $"{currentWeapon.rarityString}\n{currentWeapon.name}";
        descriptionText.text = currentWeapon.description;
    }
}
