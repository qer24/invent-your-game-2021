using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Room : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    public float maxTimeBetweenWaves = 15;

    public static List<GameObject> enemiesAlive = new List<GameObject>();
    float waveTimer;

    private void Start()
    {
        waveTimer = 3;
    }

    private void Update()
    {
        if (waves.Count < 1) return;

        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0)
        {
            NextWave();
            waveTimer = maxTimeBetweenWaves;
        }
    }

    void NextWave()
    {
        foreach (var enemySpawn in waves[0].enemiesToSpawn)
        {
            GameObject go = LeanPool.Spawn(enemySpawn.enemy, RoomManager.WorldPositionFromSpawnPoint(enemySpawn.spawnPoint), Quaternion.identity);
            enemiesAlive.Add(go);
        }
        waves.RemoveAt(0);
    }
}
