using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ProcGen;

public class PlayerHealth : Health
{
    public Stats playerStats;

    [SerializeField] Slider healthSlider = null;
    [SerializeField] float healthLerpSpeed = 3f;

    float currentFillAmount;

    public override void Awake()
    {
        stats = playerStats;

        base.Awake();
        currentFillAmount = 1;

        OnHealthChanged += UpdateUI;
        RoomManager.OnRoomComplete += () => RestoreHealth(stats.maxHealth);
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
}
