using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcGen;

namespace ProcGen
{
    public class EnemySpawn
    {
        public Enemy enemy;
        public RoomSpawnPoints spawnPoint;
        public EnemyCard enemyCard;

        public EnemySpawn(EnemyCard enemyCard, RoomSpawnPoints spawnPoint)
        {
            this.enemyCard = enemyCard;
            enemy = enemyCard.prefab;
            this.spawnPoint = spawnPoint;
        }
    }
}
