using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] ScaleTween tooltip = null;
    RectTransform tooltipRectTransform = null;

    [SerializeField] Image pickupIndicator = null;
    [SerializeField] Color highlightColor = Color.white;
    Color startingColor;

    Vector3 startPos;
    Transform startParent;

    public bool isInSlot = false;

    public static bool DraggingMod;
    public static ModDrop modBeingDragged;

    private void Start()
    {
        startingColor = pickupIndicator.color;

        tooltipRectTransform = tooltip.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(tooltip.gameObject.activeSelf)
        {
            Vector3 pos = Input.mousePosition;

            float pivotX = pos.x / Screen.width;
            float pivotY = pos.y / Screen.height;

            tooltipRectTransform.pivot = new Vector2(pivotX, pivotY);
            tooltip.transform.position = pos;
        }
    }

    private void OnDisable()
    {
        if (tooltip.gameObject.activeSelf)
        {
            tooltip.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!DraggingMod)
        {
            LeanTween.value(gameObject, (Color val) => pickupIndicator.color = val, pickupIndicator.color, highlightColor, 0.1f);
            tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!DraggingMod)
        {
            LeanTween.value(gameObject, (Color val) => pickupIndicator.color = val, pickupIndicator.color, startingColor, 0.1f);
            tooltip.gameObject.SetActive(false);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isInSlot)
        {
            eventData.pointerDrag = null;
            return;
        }

        DraggingMod = true;
        tooltip.gameObject.SetActive(false);

        modBeingDragged = this;
        startPos = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DraggingMod = false;

        modBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            LeanTween.value(gameObject, (Color val) => pickupIndicator.color = val, pickupIndicator.color, startingColor, 0.1f);
            LeanTween.move(gameObject, startPos, 0.5f).setEase(LeanTweenType.easeOutExpo);
        }
    }
}
