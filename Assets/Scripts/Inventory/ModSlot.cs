using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModSlot : MonoBehaviour, IDropHandler
{
    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    Weapon weapon;

    private void Start()
    {
        weapon = GetComponentInParent<Weapon>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {

            ModDrop draggedMod = ModDrop.modBeingDragged;

            //prevent from equipping 2 of the same mod
            var component = transform.parent.parent.GetComponentInChildren(draggedMod.GetComponent<Mod>().GetType());
            if (component != null)
            {
                return;
            }

            draggedMod.transform.SetParent(transform);
            draggedMod.isInSlot = true;
            LeanTween.move(draggedMod.gameObject, transform.position, 0.1f).setEase(LeanTweenType.easeOutExpo);

            draggedMod.GetComponent<Mod>().AttachWeapon(weapon);

            enabled = false;
        }
    }
}
