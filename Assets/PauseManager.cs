using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }
    public static bool paused = false;
    public static Action OnPause, OnUnpause;

    public GameObject pausePanel;
    public RectTransform mainPanel;
    CanvasGroup mainGroup;
    public RectTransform optionsPanel;
    CanvasGroup optionsGroup;

    public float transitionTime = 0.5f;
    public LeanTweenType transitionType;

    bool optionsToggled = false;
    float unpausedTimescale = 1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        mainGroup = mainPanel.GetComponent<CanvasGroup>();
        optionsGroup = optionsPanel.GetComponent<CanvasGroup>();

        //pausePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                Unpause();
            }else
            {
                Pause();
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus && !paused)
        {
            Pause();
        }
    }

    public void Pause()
    {
        unpausedTimescale = Time.timeScale;
        Time.timeScale = 0;
        paused = true;

        pausePanel.SetActive(true);

        OnPause?.Invoke();
    }

    public void Unpause()
    {
        Time.timeScale = unpausedTimescale;
        paused = false;

        optionsToggled = false;
        mainGroup.blocksRaycasts = true;
        optionsGroup.blocksRaycasts = false;
        LeanTween.moveX(mainPanel, 0, 0).setIgnoreTimeScale(true);
        LeanTween.moveX(optionsPanel, optionsPanel.sizeDelta.x, 0).setIgnoreTimeScale(true);

        pausePanel.SetActive(false);
        OnPause?.Invoke();
    }

    public void ToggleOptions()
    {
        LeanTween.cancel(gameObject);
        optionsToggled = !optionsToggled;

        if(optionsToggled)
        {
            mainGroup.blocksRaycasts = false;
            optionsGroup.blocksRaycasts = true;
            LeanTween.moveX(mainPanel, -mainPanel.sizeDelta.x, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
            LeanTween.moveX(optionsPanel, 0, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
        }
        else
        {
            mainGroup.blocksRaycasts = true;
            optionsGroup.blocksRaycasts = false;
            LeanTween.moveX(mainPanel, 0, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
            LeanTween.moveX(optionsPanel, optionsPanel.sizeDelta.x, transitionTime).setEase(transitionType).setIgnoreTimeScale(true);
        }
    }
}
