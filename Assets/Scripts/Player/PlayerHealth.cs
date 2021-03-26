using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProcGen;
using System;

public class PlayerHealth : Health
{
    public Stats playerStats;

    [SerializeField] Slider healthSlider = null;
    [SerializeField] float healthLerpSpeed = 3f;
    [SerializeField] GameObject OnDeathScreen = null;

    float currentFillAmount;

    public static Action OnPlayerDeath;
    public static bool IsPlayerDead = false;

    public override void Awake()
    {
        stats = playerStats;

        base.Awake();
        currentFillAmount = 1;

        OnHealthChanged += UpdateUI;
        //RoomManager.OnRoomComplete += () => RestoreHealth(stats.maxHealth);
        RoomManager.OnRoomChanged += () => RestoreHealth(stats.maxHealth);
        OnDeath += PlayerDeath;
    }

    private void Update()
    {
        float fillAmount = Mathf.Lerp(healthSlider.value, currentFillAmount, Time.deltaTime * healthLerpSpeed);
        healthSlider.value = fillAmount;
    }

    private void OnDisable()
    {
        OnHealthChanged -= UpdateUI;
    }

    void UpdateUI(float currentHealth)
    {
        currentFillAmount = currentHealth / stats.maxHealth;
    }

    void PlayerDeath()
    {
        IsPlayerDead = true;
        OnPlayerDeath?.Invoke();
        Destroy(gameObject);

        Instantiate(OnDeathScreen);
    }
}
