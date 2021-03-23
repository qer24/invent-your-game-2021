using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject loadingScreen;

    List<AsyncOperation> loadingOperations = new List<AsyncOperation>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }

        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    public void LoadGame(int levelIndex)
    {
        loadingScreen.SetActive(true);

        loadingOperations.Add(SceneManager.UnloadSceneAsync(1));
        loadingOperations.Add(SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    IEnumerator GetSceneLoadProgress()
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

        Debug.Log($"Finished loading in {Mathf.Round((Time.realtimeSinceStartup - startTime) * 1000)} ms");

        yield return new WaitForSecondsRealtime(0.5f);

        loadingOperations.Clear();
        loadingScreen.SetActive(false);

        Time.timeScale = PauseManager.unpausedTimescale;
    }
}
