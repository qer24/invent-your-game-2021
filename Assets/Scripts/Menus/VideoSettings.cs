using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ScreenResolution
{
    public int width;
    public int height;

    public ScreenResolution(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public string text { get => $"{width}x{height}"; }
}

public class VideoSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public ScreenResolution[] resolutions;
    ScreenResolution currentScreenResolution;
    [HideInInspector] public int currentResolutionIndex;
    [HideInInspector] public int defaultResolutionIndex;

    public TMP_Dropdown fullscreenDropdown;
    public VerticalLayoutGroup tabLayoutGroup;

    RectTransform resolutionDropdownRectTransform;
    float resolutionDropdownNormalY = 0;
    float resolutionDropdownPushedY = 0;

    [HideInInspector] public FullScreenMode fullscreenMode = FullScreenMode.ExclusiveFullScreen;

    public Toggle vsyncToggle;
    [HideInInspector] public int vsyncCount = 1;

    public TMP_Dropdown qualityDropdown;
    [HideInInspector] public int currentQualitySettings = 2;

    private void Start()
    {
        resolutionDropdown.ClearOptions();

        var resolutionList = new List<ScreenResolution>(resolutions);

        var defaultRes = new ScreenResolution(Screen.currentResolution.width, Screen.currentResolution.height);
        defaultResolutionIndex = 0;
        for (int i = 0; i < resolutionList.Count; i++)
        {
            if (resolutionList[i].width == defaultRes.width && resolutionList[i].height == defaultRes.height)
            {
                defaultResolutionIndex = i;
                break;
            }
        }
        if(defaultResolutionIndex == 0)
        {
            resolutionList.Insert(0, defaultRes);
        }

        var options = new List<string>();
        foreach (var res in resolutionList)
        {
            options.Add(res.text);
        }
        resolutionDropdown.AddOptions(options);

        resolutions = resolutionList.ToArray();

        resolutionDropdownRectTransform = resolutionDropdown.transform.parent.GetComponent<RectTransform>();
        resolutionDropdownNormalY = 0;
        resolutionDropdownPushedY = 0;
        Invoke(nameof(DisableLayoutGroup), 0.1f);

        fullscreenMode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenMode", 1);
        UpdateFullScreenDropdown();

        if(PlayerPrefs.HasKey("CurrentResolutionIndex"))
        {
            currentResolutionIndex = PlayerPrefs.GetInt("CurrentResolutionIndex");
            resolutionDropdown.value = currentResolutionIndex;
        }
        else
        {
            resolutionDropdown.value = defaultResolutionIndex;
            currentResolutionIndex = defaultResolutionIndex;
        }
        ChangeResolution(currentResolutionIndex);

        vsyncCount = PlayerPrefs.GetInt("Vsync", 1);
        ToggleVSync(vsyncCount == 1);
        vsyncToggle.isOn = vsyncCount == 1;

        SetQuality(PlayerPrefs.GetInt("Quality", 2));
        qualityDropdown.value = currentQualitySettings;
    }

    void DisableLayoutGroup()
    {
        tabLayoutGroup.enabled = false;

        resolutionDropdownNormalY = resolutionDropdownRectTransform.anchoredPosition3D.y;
        resolutionDropdownPushedY = resolutionDropdownRectTransform.anchoredPosition3D.y - fullscreenDropdown.template.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void Update()
    {
        if (resolutionDropdownPushedY == resolutionDropdownNormalY) return;

        if (fullscreenDropdown.transform.childCount != 3)
        {
            resolutionDropdownRectTransform.anchoredPosition = new Vector2(resolutionDropdownRectTransform.anchoredPosition.x, resolutionDropdownPushedY);
        }
        else
        {
            resolutionDropdownRectTransform.anchoredPosition = new Vector2(resolutionDropdownRectTransform.anchoredPosition.x, resolutionDropdownNormalY);
        }
    }

    public void UpdateFullScreenDropdown()
    {
        switch (fullscreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                fullscreenDropdown.value = 0;
                break;
            case FullScreenMode.FullScreenWindow:
                fullscreenDropdown.value = 1;
                break;
            case FullScreenMode.Windowed:
                fullscreenDropdown.value = 2;
                break;
        }
    }

    public void ChangeFullscreenMode(int index)
    {
        switch(index)
        {
            case 0:
                fullscreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                fullscreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                fullscreenMode = FullScreenMode.Windowed;
                break;
        }

        Screen.fullScreenMode = fullscreenMode;

        PlayerPrefs.SetInt("FullscreenMode", (int)fullscreenMode);
    }

    public void ChangeResolution(int resIndex)
    {
        currentResolutionIndex = resIndex;
        currentScreenResolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(currentScreenResolution.width, currentScreenResolution.height, fullscreenMode, Screen.currentResolution.refreshRate);

        PlayerPrefs.SetInt("CurrentResolutionIndex", currentResolutionIndex);
    }

    public void ToggleVSync(bool vsync)
    {
        vsyncCount = vsync ? 1 : 0;
        QualitySettings.vSyncCount = vsyncCount;

        PlayerPrefs.SetInt("Vsync", vsyncCount);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        currentQualitySettings = qualityIndex;

        PlayerPrefs.SetInt("Quality", qualityIndex);
    }
}
                  