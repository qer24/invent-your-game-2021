using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class PlayerUpgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image backgroundImage, mainImage;
    public Color baseColor, hoverColor, unlockedColor;
    bool unlocked = false;

    

    private void Start()
    {
        mainImage.color = baseColor;
        backgroundImage.color = baseColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (unlocked) return;

        LeanTween.value(gameObject, (Color val) => mainImage.color = val, mainImage.color, unlockedColor, 0.25f);
        LeanTween.value(gameObject, (Color val) => backgroundImage.color = val, backgroundImage.color, unlockedColor, 0.25f);

        unlocked = true;

        float time = 0.25f;
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), time).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            LeanTween.scale(gameObject, Vector3.one, time * 2f).setEase(LeanTweenType.easeInCubic);
        }
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unlocked) return;

        LeanTween.value(gameObject, (Color val) => mainImage.color = val, mainImage.color, hoverColor, 0.25f);
        LeanTween.value(gameObject, (Color val) => backgroundImage.color = val, backgroundImage.color, hoverColor, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (unlocked) return;

        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, (Color val) => mainImage.color = val, mainImage.color, baseColor, 0.25f);
        LeanTween.value(gameObject, (Color val) => backgroundImage.color = val, backgroundImage.color, baseColor, 0.25f);
    }
}
