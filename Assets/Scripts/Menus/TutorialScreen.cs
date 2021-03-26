using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcGen;

public class TutorialScreen : MonoBehaviour
{
    private void Start()
    {
        RoomManager.OnRoomChanged += Close;
    }

    private void OnDisable()
    {
        RoomManager.OnRoomChanged -= Close;
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
