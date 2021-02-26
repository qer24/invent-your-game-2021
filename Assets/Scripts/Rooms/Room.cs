using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using ProcGen;
using System;
using Random = UnityEngine.Random;

namespace ProcGen
{
    public class Room : MonoBehaviour
    {
        List<ProceduralWave> waves = new List<ProceduralWave>();
        List<Wave> roomWaves = new List<Wave>();
        public float maxTimeBetweenWaves = 15;

        public ProceduralRoom mapRoom = null;

        RoomManager roomManager;
        int id = 0;

        public static List<GameObject> enemiesAlive = new List<GameObject>();
        float waveTimer;

        bool waveFinished = false;
        public Action OnRoomCompleted;

        public List<GameObject> dropsInThisRoom;

        bool completed = false;

        private void Start()
        {
            roomManager = GetComponentInParent<RoomManager>();
            id = GetComponent<RoomContentGenerator>().id;

            waveTimer = 3;
            waveFinished = true;

            enemiesAlive = new List<GameObject>();
            roomWaves = waves;

            dropsInThisRoom = new List<GameObject>();

            completed = false;
            enabled = false;
        }

        public void DisableDrops()
        {
            foreach (var drop in dropsInThisRoom)
            {
                if (drop.transform.parent != DropManager.Instance.transform) continue;

                LeanTween.scale(drop, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack).setOnComplete(() => Destroy(drop));
            }
        }

        private void Update()
        {
            if (completed) return;

            for (int i = 0; i < enemiesAlive.Count; i++)
            {
                if (enemiesAlive[i] == null)
                {
                    enemiesAlive.RemoveAt(i);
                }
            }

            if (roomWaves.Count < 1)
            {
                if (enemiesAlive.Count <= 0)
                {
                    CompleteRoom();
                }
                return;
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

        private void CompleteRoom()
        {
            completed = true;

            if (id != 0)
            {
                if (Random.value > 0.5f)
                {
                    dropsInThisRoom.Add(DropManager.Instance.DropWeapon());
                }
                else
                {
                    dropsInThisRoom.Add(DropManager.Instance.DropMod());
                }
            }

            OnRoomCompleted?.Invoke();
        }

        public void GoToThisRoom()
        {
            roomManager.GoToRoom(id);
        }

        void NextWave()
        {
            waveFinished = false;

            foreach (var enemySpawn in roomWaves[0].enemiesToSpawn)
            {
                GameObject go = Instantiate(enemySpawn.enemy, roomManager.WorldPositionFromSpawnPoint(enemySpawn.spawnPoint), Quaternion.identity);
                enemiesAlive.Add(go);
            }
            roomWaves.RemoveAt(0);
        }
    }
}
