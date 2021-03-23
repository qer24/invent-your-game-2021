using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [EventRef] public string sfxTest;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public float masterVolume = 1f;

    FMOD.Studio.Bus Master;
    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus SFX;

    [SerializeField] Slider masterSlider = null;
    [SerializeField] Slider musicSlider = null;
    [SerializeField] Slider sfxSlider = null;

    [SerializeField] Toggle muteInBackgroundToggle = null;

    private void Awake()
    {
        Music = RuntimeManager.GetBus("bus:/Master/Music");
        SFX = RuntimeManager.GetBus("bus:/Master/SFX");
        Master = RuntimeManager.GetBus("bus:/Master");

        SetVolume();
        UpdateUI();
    }

    public void SetVolume()
    {
        Music.setVolume(musicVolume);
        SFX.setVolume(sfxVolume);
        Master.setVolume(masterVolume);
    }

    public void UpdateMusicVolume(float vol)
    {
        musicVolume = vol;
        SetVolume();
    }

    public void UpdateSFXVolume(float vol)
    {
        if (Time.time > 2f)
            AudioManager.Play(sfxTest, true);
        sfxVolume = vol;
        SetVolume();
    }

    public void UpdateMasterVolume(float vol)
    {
        masterVolume = vol;
        SetVolume();
    }

    public void UpdateMuteInBackground(bool mute)
    {
        BackgroundAudioManager.muteInBackground = mute;
    }

    void OnEnable()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVol", 0.75f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVol", 0.75f);
        musicVolume = PlayerPrefs.GetFloat("MusicVol", 0.75f);

        SetVolume();
        UpdateUI();
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat("MasterVol", masterVolume);
        PlayerPrefs.SetFloat("SFXVol", sfxVolume);
        PlayerPrefs.SetFloat("MusicVol", musicVolume);

        SetVolume();
        UpdateUI();
    }

    public void UpdateUI()
    {
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;
        masterSlider.value = masterVolume;
        muteInBackgroundToggle.isOn = BackgroundAudioManager.muteInBackground;
    }
}
