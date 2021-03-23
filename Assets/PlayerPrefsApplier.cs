using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsApplier : MonoBehaviour
{
    public VolumeSettings volumeSettings;
    public VideoSettings videoSettings;
    public GameplaySettings gameplaySettings;

    public void ResetToDefault()
    {
        PlayerPrefs.SetFloat("MasterVol", 0.75f);
        PlayerPrefs.SetFloat("SFXVol", 0.75f);
        PlayerPrefs.SetFloat("MusicVol", 0.75f);
        PlayerPrefs.SetInt("MuteInBackground", 1);

        volumeSettings.masterVolume = 0.75f;
        volumeSettings.sfxVolume = 0.75f;
        volumeSettings.musicVolume = 0.75f;

        volumeSettings.SetVolume();
        volumeSettings.UpdateMuteInBackground(true);

        volumeSettings.UpdateUI();

        PlayerPrefs.SetInt("FullscreenMode", 1);
        PlayerPrefs.SetInt("CurrentResolutionIndex", videoSettings.defaultResolutionIndex);
        PlayerPrefs.SetInt("Vsync", 1);
        PlayerPrefs.SetInt("Quality", 2);

        videoSettings.fullscreenMode = (FullScreenMode)1;
        videoSettings.UpdateFullScreenDropdown();

        videoSettings.resolutionDropdown.value = videoSettings.defaultResolutionIndex;
        videoSettings.ChangeResolution(videoSettings.defaultResolutionIndex);

        videoSettings.ToggleVSync(true);
        videoSettings.vsyncToggle.isOn = true;

        videoSettings.SetQuality(2);
        videoSettings.qualityDropdown.value = 2;

        PlayerPrefs.SetInt("SelectedLanguage", 0);
        PlayerPrefs.SetInt("ScreenShake", 1);

        gameplaySettings.languageDropdown.value = 0;
        gameplaySettings.ToggleScreenShake(true);
        gameplaySettings.screenShakeToggle.isOn = true;

        PlayerPrefs.Save();
    }

    public void Apply()
    {
        PlayerPrefs.SetFloat("MasterVol", volumeSettings.masterVolume);
        PlayerPrefs.SetFloat("SFXVol", volumeSettings.sfxVolume);
        PlayerPrefs.SetFloat("MusicVol", volumeSettings.musicVolume);
        PlayerPrefs.SetInt("MuteInBackground", BackgroundAudioManager.MuteInBackground ? 1 : 0);

        PlayerPrefs.SetInt("FullscreenMode", (int)videoSettings.fullscreenMode);
        PlayerPrefs.SetInt("CurrentResolutionIndex", videoSettings.currentResolutionIndex);
        PlayerPrefs.SetInt("Vsync", videoSettings.vsyncCount);
        PlayerPrefs.SetInt("Quality", videoSettings.currentQualitySettings);

        PlayerPrefs.SetInt("SelectedLanguage", gameplaySettings.selectedLanguageIndex);
        PlayerPrefs.SetInt("ScreenShake", GameplaySettings.DoScreenShake ? 1 : 0);

        PlayerPrefs.Save();
    }
}
