using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProcGen;
using System;
using TMPro;

public class PlayerUpgradeManager : MonoBehaviour
{
    public FolowMouse mousePos = null;
    [SerializeField] CanvasGroup buttonCanvasGroup = null;
    [SerializeField] Image levelUpBar = null; 
    [SerializeField] ScaleTween upgradePanel = null;

    [SerializeField, FMODUnity.EventRef] string levelUpSound = null;

    PlayerUpgrade[] allUpgrades;
    ScaleTween buttonScaleTween;
    TextMeshProUGUI levelUpBarText;
    Button button;
    float alphaTime = 1;

    public static bool IsPanelOpen;
    public static PlayerUpgradeManager Instance;

    int currentExp = 0;
    int expToNextLevel = 15;
    int lastExpToLevel = 0;
    [HideInInspector] public int levelUpPoints = 0;
    bool readyToLevelUp = false;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }else
        {
            Destroy(this);
        }

        buttonScaleTween = buttonCanvasGroup.GetComponent<ScaleTween>();
        levelUpBarText = levelUpBar.GetComponentInChildren<TextMeshProUGUI>();
        button = buttonCanvasGroup.GetComponent<Button>();

        allUpgrades = upgradePanel.GetComponentsInChildren<PlayerUpgrade>();

        RoomManager.OnRoomChanged += DisableButton;
        RoomManager.OnRoomComplete += EnableButton;
        RoomManager.OnRoomChanged += () => 
        {
            IsPanelOpen = false;

            upgradePanel.Close();
            foreach (var upgrade in allUpgrades)
            {
                upgrade.interactable = false;
            }
        };

        IsPanelOpen = false;
    }

    void Update()
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

        if(upgradePanel.gameObject.activeSelf)
        {
            upgradePanel.gameObject.SetActive(false);
            LeanTween.cancel(upgradePanel.gameObject);
        }
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

        EnableButton();
    }

    public void EnableButton()
    {
        if (!readyToLevelUp) return;

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

    public void AddExp(int amount)
    {
        currentExp += amount;

        if(currentExp >= expToNextLevel)
        {
            LevelUp();
        }
        if(levelUpPoints > 0)
        {
            readyToLevelUp = true;
        }

        float fill = (float)(currentExp - lastExpToLevel) / (expToNextLevel - lastExpToLevel);
        levelUpBar.fillAmount = fill;
    }

    void LevelUp()
    {
        AudioManager.Play(levelUpSound, true);

        levelUpPoints += 1;
        lastExpToLevel = expToNextLevel;
        expToNextLevel *= 3;

        levelUpBarText.text = levelUpPoints.ToString();
        levelUpBar.fillAmount = 0;
    }

    public void UseLevelUpPoint()
    {
        levelUpPoints -= 1;
        if(levelUpPoints == 0)
        {
            readyToLevelUp = false;
        }
        levelUpBarText.text = levelUpPoints.ToString();
    }
}
