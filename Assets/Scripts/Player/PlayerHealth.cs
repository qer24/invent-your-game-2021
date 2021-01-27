using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [SerializeField] Slider healthSlider = null;
    [SerializeField] float healthLerpSpeed = 3f;

    float currentFillAmount;

    public override void Start()
    {
        currentHealth = stats.maxHealth;
        isDead = false;
        currentFillAmount = 1;

        OnHealthChanged += UpdateUI;
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
