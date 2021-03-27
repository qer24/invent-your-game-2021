using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject loadingScreen;

    public static List<AsyncOperation> loadingOperations = new List<AsyncOperation>();

    public static bool LoadedGame = false;

    IEnumerator Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            yield break;
        }

        LoadedGame = false;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    public void LoadGame(int levelIndex)
    {
        loadingScreen.SetActive(true);

        loadingOperations.Add(SceneManager.UnloadSceneAsync(1));
        loadingOperations.Add(SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadLevel()
    {
        loadingScreen.SetActive(true);

        loadingOperations.Add(SceneManager.UnloadSceneAsync(2));
        loadingOperations.Add(SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress(true));
    }

    IEnumerator GetSceneLoadProgress(bool loadingLevel = false)
    {
        PauseManager.unpausedTimescale = Time.timeScale;
        Time.timeScale = 0;

        var startTime = Time.realtimeSinceStartup;

        for (int i = 0; i < loadingOperations.Count; i++)
        {
            while (!loadingOperations[i].isDone) yield return null;
        }

        while (!ProcGen.LevelGeneration.doneGenerating)
        {
            yield return null;
        }

        Debug.Log($"Finished generating map in {Mathf.Round((Time.realtimeSinceStartup - startTime) * 1000)} ms");

        yield return new WaitForSecondsRealtime(0.5f);

        loadingOperations.Clear();
        loadingScreen.SetActive(false);

        if (!loadingLevel)
        {
            DropManager.ResetDrops();
            EndScreen.ResetStats();
        }
        ProcGen.Room.enemiesAlive = new List<GameObject>();
        PlayerHealth.IsPlayerDead = false;

        Time.timeScale = PauseManager.unpausedTimescale;
        LoadedGame = true;
    }
}
