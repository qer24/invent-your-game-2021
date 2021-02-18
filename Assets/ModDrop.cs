using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ModDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] ScaleTween tooltip = null;

    [SerializeField] TextMeshProUGUI nameText = null;
    [SerializeField] TextMeshProUGUI descriptionText = null;

    [SerializeField] Image pickupIndicator = null;
    [SerializeField] Color highlightColor = Color.white;
    Color startingColor;

    Vector3 startPos;
    private bool dragging;

    private void Start()
    {
        startingColor = pickupIndicator.color;
    }

    public void Update()
    {
        if (dragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
        startPos = transform.position;
        dragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        LeanTween.value(gameObject, (Color val) => pickupIndicator.color = val, pickupIndicator.color, startingColor, 0.1f);
        LeanTween.move(gameObject, startPos, 0.5f).setEase(LeanTweenType.easeOutExpo);

        dragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!dragging)
        {
            LeanTween.value(gameObject, (Color val) => pickupIndicator.color = val, pickupIndicator.color, highlightColor, 0.1f);
            tooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!dragging)
        {
            LeanTween.value(gameObject, (Color val) => pickupIndicator.color = val, pickupIndicator.color, startingColor, 0.1f);
            tooltip.gameObject.SetActive(false);
        }
    }
}
