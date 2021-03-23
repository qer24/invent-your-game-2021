using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    float time;

    private void Awake()
    {
        Music = RuntimeManager.GetBus("bus:/Master/Music");
        SFX = RuntimeManager.GetBus("bus:/Master/SFX");
        Master = RuntimeManager.GetBus("bus:/Master");

        SetVolume();
        UpdateUI();

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) => 
        {
            time = 0;
        };
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

    private void Update()
    {
        if (time < 1) time += Time.deltaTime;
    }

    public void UpdateSFXVolume(float vol)
    {
        if(time >= 1)
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
        BackgroundAudioManager.MuteInBackground = mute;
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
        muteInBackgroundToggle.isOn = BackgroundAudioManager.MuteInBackground;
    }
}
