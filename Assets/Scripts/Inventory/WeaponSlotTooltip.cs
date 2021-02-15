using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlotTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Weapon currentWeapon = null;
    [SerializeField] ScaleTween tooltip = null;

    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI descriptionText = null;

    private void OnEnable()
    {
        UpdateText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }

    void UpdateText()
    {
        nameText.text = currentWeapon.name;
        descriptionText.text = currentWeapon.description;
    }
}
