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

        pausePanel.SetActive(false);
        OnPause?.Invoke();
    }
}
