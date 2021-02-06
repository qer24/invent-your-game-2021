using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using ProcGen;

public class Room : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    public float maxTimeBetweenWaves = 15;

    public ProceduralRoom mapRoom = null;

    RoomManager roomManager;
    int id = 0;

    public static List<GameObject> enemiesAlive = new List<GameObject>();
    float waveTimer;

    bool waveFinished = false;

    private void Start()
    {
        roomManager = GetComponentInParent<RoomManager>();
        id = GetComponent<RoomContentGenerator>().id;

        waveTimer = 3;
        waveFinished = true;

        enemiesAlive = new List<GameObject>();

        enabled = false;
    }

    private void Update()
    {
        if (waves.Count < 1) return;

        for (int i = 0; i < enemiesAlive.Count; i++)
        {
            if (enemiesAlive[i] == null)
            {
                enemiesAlive.RemoveAt(i);
            }
        }

        if (!waveFinished && enemiesAlive.Count <= 0)
        {
            waveFinished = true;
            waveTimer = 0.1f * maxTimeBetweenWaves;
        }

        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0)
        {
            NextWave();
            waveTimer = maxTimeBetweenWaves;
        }
    }

    public void GoToThisRoom()
    {
        roomManager.GoToRoom(id);
    }

    void NextWave()
    {
        waveFinished = false;

        foreach (var enemySpawn in waves[0].enemiesToSpawn)
        {
            GameObject go = Instantiate(enemySpawn.enemy, roomManager.WorldPositionFromSpawnPoint(enemySpawn.spawnPoint), Quaternion.identity);
            enemiesAlive.Add(go);
        }
        waves.RemoveAt(0);
    }
}
