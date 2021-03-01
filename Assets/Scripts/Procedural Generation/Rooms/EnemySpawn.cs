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
        public int expValue;

        public EnemySpawn(EnemyCard enemyCard, RoomSpawnPoints spawnPoint, int expValue)
        {
            this.enemyCard = enemyCard;
            enemy = enemyCard.prefab;
            this.spawnPoint = spawnPoint;
            this.expValue = expValue;
        }
    }
}
