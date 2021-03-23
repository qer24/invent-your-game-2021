using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class HorizontalDropdown : MonoBehaviour
{
    TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void NextItem()
    {
        int nextValue = dropdown.value + 1;
        if(nextValue >= dropdown.options.Count)
        {
            nextValue = 0;
        }

        dropdown.value = nextValue;
    }

    public void PreviousItem()
    {
        int previousValue = dropdown.value - 1;
        if (previousValue < 0)
        {
            previousValue = dropdown.options.Count;
        }

        dropdown.value = previousValue;
    }
}
