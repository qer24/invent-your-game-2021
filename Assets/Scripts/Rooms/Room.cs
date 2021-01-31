using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Waves to spawn")]
    public List<Wave> waves = new List<Wave>();

    [Space]
    [Header("RoomSettings")]
    public float maxTimeBetweenWaves = 15;
}
