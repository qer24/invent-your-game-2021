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

    [HideInInspector]public int currentExp = 0;
    int expToNextLevel = 15;
    int lastExpToLevel = 0;
    [HideInInspector] public int levelUpPoints = 0;
    bool readyToLevelUp = false;

    [HideInInspector] public int currentLevel = 1;

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
        RoomManager.OnRoomChanged += DisablePanel;

        IsPanelOpen = false;
        upgradePanel.gameObject.SetActive(false);
    }

    private void DisablePanel()
    {
        IsPanelOpen = false;

        upgradePanel.Close();
        foreach (var upgrade in allUpgrades)
        {
            upgrade.interactable = false;
        }

        PlayerGUIOrderManager.Instance.ShowOnTop(false);
    }

    private void OnDisable()
    {
        RoomManager.OnRoomChanged -= DisableButton;
        RoomManager.OnRoomComplete -= EnableButton;
        RoomManager.OnRoomChanged -= DisablePanel;
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

        PlayerGUIOrderManager.Instance.ShowOnTop(true);
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

        PlayerGUIOrderManager.Instance.ShowOnTop(false);
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
        if(levelUpBar != null)
            levelUpBar.fillAmount = fill;

        EndScreen.ExpGained += amount;
    }

    void LevelUp()
    {
        AudioManager.Play(levelUpSound, true);

        levelUpPoints += 1;
        lastExpToLevel = expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 3.5f);

        if(levelUpBar != null)
        {
            levelUpBarText.text = levelUpPoints.ToString();
            levelUpBar.fillAmount = 0;
        }

        currentLevel++;
        EndScreen.LevelsGained++;
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
