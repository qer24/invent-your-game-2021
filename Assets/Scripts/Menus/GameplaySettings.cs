using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GameplaySettings : MonoBehaviour
{
    public static bool DoScreenShake = true;

    public Toggle screenShakeToggle;

    public int selectedLanguageIndex = 0;
    public TMP_Dropdown languageDropdown;

    static bool initialized = false;

    IEnumerator Start()
    {
        DoScreenShake = PlayerPrefs.GetInt("ScreenShake", 1) == 1;
        screenShakeToggle.isOn = DoScreenShake;

        if (!initialized)
        {
            // Wait for the localization system to initialize, loading Locales, preloading etc.
            yield return LocalizationSettings.InitializationOperation;
            initialized = true;
        }

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("SelectedLanguage", 0)];

        // Generate list of available Locales
        var options = new List<TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMP_Dropdown.OptionData(locale.Identifier.CultureInfo.NativeName.Split('(')[0].FirstCharToUpper()));
        }
        languageDropdown.options = options;

        languageDropdown.value = selected;
        selectedLanguageIndex = selected;
        languageDropdown.onValueChanged.AddListener(LocaleSelected);
    }

    void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        selectedLanguageIndex = index;

        PlayerPrefs.SetInt("SelectedLanguage", index);
    }

    public void ToggleScreenShake(bool shake)
    {
        DoScreenShake = shake;
        PlayerPrefs.SetInt("ScreenShake", shake ? 1 : 0);
    }
}
