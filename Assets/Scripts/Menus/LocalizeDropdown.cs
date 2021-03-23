using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

[RequireComponent(typeof(TMP_Dropdown))]
public class LocalizeDropdown : MonoBehaviour
{
    TMP_Dropdown dropdown;
    [SerializeField] LocalizedString stringReference = new LocalizedString();

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    private void OnEnable()
    {
        StartCoroutine(GetLocalizedDropdown());
    }

    IEnumerator GetLocalizedDropdown()
    {
        // Wait for the localization system to initialize, loading Locales, preloading etc.
        yield return LocalizationSettings.InitializationOperation;

        var localizedText = stringReference.GetLocalizedString();

        if (localizedText.IsDone)
        {
            UpdateDropdownText(localizedText.Result);
        }
        else
        {
            localizedText.Completed += (o) => UpdateDropdownText(localizedText.Result);
        }
    }

    void UpdateDropdownText(string text)
    {
        string[] split = text.Split('|');
        for (int i = 0; i < split.Length; i++)
        {
            dropdown.options[i].text = split[i];
        }
        dropdown.RefreshShownValue();
    }
}
