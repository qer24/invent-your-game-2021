using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorialManager : MonoBehaviour
{
    public static PlayerTutorialManager Instance;

    public static bool RoomTutorialCompleted;
    public static bool ModTutorialCompleted;

    public GameObject roomTutorialPrefab;
    public GameObject modTutorialPrefab;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        RoomTutorialCompleted = PlayerPrefs.GetInt("RoomTutorialCompleted", 0) == 1;
        ModTutorialCompleted = PlayerPrefs.GetInt("ModTutorialCompleted", 0) == 1;

        Invoke(nameof(ShowRoomTutorial), 2f);
        DropManager.OnFirstModDrop += ShowModTutorial;
    }

    private void OnDisable()
    {
        DropManager.OnFirstModDrop -= ShowModTutorial;
    }

    public void ShowRoomTutorial()
    {
        if (RoomTutorialCompleted) return;

        Instantiate(roomTutorialPrefab);
        PlayerPrefs.SetInt("RoomTutorialCompleted", 1);
    }

    public void ShowModTutorial()
    {
        if (ModTutorialCompleted) return;

        Instantiate(modTutorialPrefab);
        PlayerPrefs.SetInt("ModTutorialCompleted", 1);
    }
}
