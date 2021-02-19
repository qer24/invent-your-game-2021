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

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            ModDrop.modBeingDragged.transform.SetParent(transform);
            ModDrop.modBeingDragged.isInSlot = true;
            LeanTween.move(ModDrop.modBeingDragged.gameObject, transform.position, 0.1f).setEase(LeanTweenType.easeOutExpo);
            enabled = false;
        }
    }
}
