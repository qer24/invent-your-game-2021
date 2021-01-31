using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Waves to spawn")]
    [SerializeField] Wave[] waves;

    [Space]
    [Header("RoomSettings")]
    [SerializeField] float maxTimeBetweenWaves = 15;
}
