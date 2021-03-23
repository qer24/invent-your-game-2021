using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Level currentLevel;

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

        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode loadSceneMode) => 
        {
            try
            {
                currentLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
            }catch(Exception)
            {
                Debug.LogWarning("Didn't find level");
            }
        };
    }
}
