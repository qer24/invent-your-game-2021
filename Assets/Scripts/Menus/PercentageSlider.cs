using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PercentageSlider : MonoBehaviour
{
    public TextMeshProUGUI percentageText;

    private void Start()
    {
        UpdateText(GetComponent<Slider>().value);
    }

    public void UpdateText(float newValue)
    {
        var percentValue = $"{(int)(newValue * 100f)}%";
        percentageText.text = percentValue;
    }
}
