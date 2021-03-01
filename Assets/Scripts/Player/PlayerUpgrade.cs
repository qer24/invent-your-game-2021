using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class PlayerUpgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image backgroundImage, mainImage;
    public Color baseColor, hoverColor, unlockedColor;
    public ScaleTween tooltip = null;

    public bool interactable = true;
    bool unlocked = false;

    RectTransform tooltipRectTransform = null;
    Transform mousePos;
    Camera mainCam;

    protected PlayerUpgradeManager upgradeManager;
    protected PlayerController playerController;
    protected PlayerShooter playerShooter;

    private void Start()
    {
        mainImage.color = baseColor;
        backgroundImage.color = baseColor;

        tooltipRectTransform = tooltip.GetComponent<RectTransform>();
        upgradeManager = GetComponentInParent<PlayerUpgradeManager>();
        mousePos = upgradeManager.mousePos.transform;

        playerController = upgradeManager.GetComponent<PlayerController>();
        playerShooter = upgradeManager.GetComponent<PlayerShooter>();

        mainCam = Camera.main;

        tooltip.transform.SetParent(transform.parent.parent);
    }

    private void Update()
    {
        if (tooltip.gameObject.activeSelf)
        {
            Vector3 pos = Input.mousePosition;

            tooltip.transform.position = Vector3.Lerp(tooltip.transform.position, pos, Time.deltaTime * 25f);
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
        tooltip.gameObject.SetActive(true);
        tooltip.transform.position = Input.mousePosition;

        if (unlocked || !interactable || upgradeManager.levelUpPoints <= 0) return;

        LeanTween.value(gameObject, (Color val) => mainImage.color = val, mainImage.color, hoverColor, 0.25f);
        LeanTween.value(gameObject, (Color val) => backgroundImage.color = val, backgroundImage.color, hoverColor, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);

        if (unlocked || !interactable || upgradeManager.levelUpPoints <= 0) return;

        LeanTween.cancel(gameObject);

        LeanTween.value(gameObject, (Color val) => mainImage.color = val, mainImage.color, baseColor, 0.25f);
        LeanTween.value(gameObject, (Color val) => backgroundImage.color = val, backgroundImage.color, baseColor, 0.25f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (unlocked || !interactable || upgradeManager.levelUpPoints <= 0) return;

        tooltip.gameObject.SetActive(false);

        LeanTween.value(gameObject, (Color val) => mainImage.color = val, mainImage.color, unlockedColor, 0.25f);
        LeanTween.value(gameObject, (Color val) => backgroundImage.color = val, backgroundImage.color, unlockedColor, 0.25f);

        unlocked = true;

        float time = 0.25f;
        LeanTween.scale(gameObject, new Vector3(1.2f, 1.2f, 1.2f), time).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
        {
            LeanTween.scale(gameObject, Vector3.one, time * 2f).setEase(LeanTweenType.easeInCubic);
        }
        );

        Upgrade();
    }

    public virtual void Upgrade()
    {
        upgradeManager.UseLevelUpPoint();
    }
}
