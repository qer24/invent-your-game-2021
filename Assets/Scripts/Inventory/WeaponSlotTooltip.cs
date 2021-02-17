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
        if (currentWeapon != null)
        {
            ConnectWeapon();
        }
    }

    private void OnDisable()
    {
        if (currentWeapon != null)
        {
            DisconnectWeapon();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentWeapon != null)
        {
            UpdateUI();
            tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentWeapon != null)
        {
            UpdateUI();
            tooltip.gameObject.SetActive(false);
        }
    }

    public void ConnectWeapon()
    {
        UpdateUI();
        currentWeapon.OnTooltipUpdate += UpdateUI;
    }

    public void DisconnectWeapon()
    {
        currentWeapon.OnTooltipUpdate -= UpdateUI;
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
        Debug.Log($"{currentWeapon.rarityString}{currentWeapon.rodzajnikString}\n{currentWeapon.name}");
        nameText.text = $"{currentWeapon.rarityString}{currentWeapon.rodzajnikString}\n{currentWeapon.name}";
        descriptionText.text = currentWeapon.description;

    }
}
