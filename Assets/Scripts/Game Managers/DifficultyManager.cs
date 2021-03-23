using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcGen;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    public int currentDifficulty = 1;
    int roomsCompleted = -1;

    private void Start()
    {
        roomsCompleted = -1;
        RoomManager.OnRoomComplete += () =>
        {
            roomsCompleted++;
            if(roomsCompleted % 3 == 0 && roomsCompleted != 0)
            {
                currentDifficulty++;
            }
        };
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void Restart()
    {
        currentDifficulty = 1;
        roomsCompleted = -1;
    }
}
