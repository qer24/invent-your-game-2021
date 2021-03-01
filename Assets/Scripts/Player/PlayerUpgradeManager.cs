using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProcGen;

public class PlayerUpgradeManager : MonoBehaviour
{
    public FolowMouse mousePos = null;
    [SerializeField] CanvasGroup buttonCanvasGroup = null;
    [SerializeField] ScaleTween upgradePanel = null;
    PlayerUpgrade[] allUpgrades;
    ScaleTween buttonScaleTween;
    Button button;

    float alphaTime = 1;

    public static bool IsPanelOpen;

    void Start()
    {
        buttonScaleTween = buttonCanvasGroup.GetComponent<ScaleTween>();
        button = buttonCanvasGroup.GetComponent<Button>();

        allUpgrades = upgradePanel.GetComponentsInChildren<PlayerUpgrade>();

        RoomManager.OnRoomChanged += DisableButton;
        RoomManager.OnRoomComplete += EnableButton;

        IsPanelOpen = false;
    }

    private void Update()
    {
        if(buttonCanvasGroup.gameObject.activeSelf)
        {
            alphaTime += Time.deltaTime;
            buttonCanvasGroup.alpha = Mathf.Clamp((Mathf.Sin(alphaTime) + 1f) * 0.5f, 0.1f, 1f);
        }
    }

    public void OpenUpgradePanel()
    {
        IsPanelOpen = true;

        upgradePanel.gameObject.SetActive(true);
        foreach (var upgrade in allUpgrades)
        {
            upgrade.interactable = true;
        }
    }

    public void CloseUpgradePanel()
    {
        IsPanelOpen = false;

        upgradePanel.Close();
        foreach (var upgrade in allUpgrades)
        {
            upgrade.interactable = false;
        }
    }

    public void EnableButton()
    {
        button.interactable = true;

        alphaTime = 1;
        buttonCanvasGroup.gameObject.SetActive(true);
    }

    public void DisableButton()
    {
        button.interactable = false;

        alphaTime = 1;
        buttonScaleTween.Close();
    }
}
