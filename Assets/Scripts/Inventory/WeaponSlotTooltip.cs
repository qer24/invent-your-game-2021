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
            currentWeapon.OnTooltipUpdate += UpdateUI;
            Debug.Log("subscribed");
            Debug.Log(currentWeapon.OnTooltipUpdate.GetInvocationList()[0].Method.Name);
        }
    }

    private void OnDisable()
    {
        if (currentWeapon != null)
        {
            currentWeapon.OnTooltipUpdate -= UpdateUI;
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
            tooltip.gameObject.SetActive(false);
        }
    }

    public void ConnectWeapon()
    {
        currentWeapon.OnTooltipUpdate += UpdateUI;
        Debug.Log("connected");
        UpdateUI();
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
        string weaponName = $"{currentWeapon.rarityString}{currentWeapon.rodzajnikString}\n{currentWeapon.name}";
        Debug.Log(currentWeapon.name);
        nameText.text = weaponName;
        descriptionText.text = currentWeapon.description;
    }
}
