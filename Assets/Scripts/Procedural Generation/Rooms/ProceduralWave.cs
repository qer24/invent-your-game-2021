using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGen
{
    public class ProceduralWave
    {
        int difficulty;
        EnemyCard[] enemyCards;
        int creditsLeft;

        public List<EnemySpawn> EnemiesToSpawn { get; private set; }

        public ProceduralWave(int _difficulty, EnemyCard[] _enemyCards, int _credits)
        {
            difficulty = _difficulty;
            enemyCards = _enemyCards;
            creditsLeft = _credits;

            GenerateWave();
        }

        void GenerateWave()
        {
            int totalWeight = 0;
            foreach (var card in enemyCards)
            {
                totalWeight += card.weight;
            }

            int iterations = 50;
            EnemyCard lastChosenCard = null;
            EnemiesToSpawn = new List<EnemySpawn>();
            List<RoomSpawnPoints> takenSpawnPoints = new List<RoomSpawnPoints>();
            while(creditsLeft > 0 && iterations > 0)
            {
                //make sure we don't spawn same enemy twice
                var excludedList = new List<EnemyCard>(enemyCards);
                excludedList.Remove(lastChosenCard);
                //get random enemy
                EnemyCard randomEnemy = GetRandomWeightedEnemy(excludedList.ToArray());
                //if can afford
                int cost = randomEnemy.cost;
                if(cost <= creditsLeft)
                {
                    var spawnPoint = GetRandomSpawnPointExcludingList(takenSpawnPoints);
                    takenSpawnPoints.Add(spawnPoint);

                    //add him to the spawn list
                    EnemiesToSpawn.Add(new EnemySpawn(randomEnemy, spawnPoint, cost));
                    creditsLeft -= cost;
                    lastChosenCard = randomEnemy;
                }

                iterations--;
            }
        }

        RoomSpawnPoints GetRandomSpawnPointExcludingList(List<RoomSpawnPoints> list)
        {
            int iterations = 20;
            while (iterations > 0)
            {
                var spawnPoint = (RoomSpawnPoints)Random.Range(0, 20);
                if (!list.Contains(spawnPoint))
                {
                    return spawnPoint;
                }
            }

            return RoomSpawnPoints.Random;
        }

        EnemyCard GetRandomWeightedEnemy(EnemyCard[] cards)
        {
            int weightSum = 0;
            foreach (var enemy in cards)
            {
                weightSum += enemy.weight;
            }

            // Step through all the possibilities, one by one, checking to see if each one is selected.
            int index = 0;
            int lastIndex = cards.Length - 1;
            while (index < lastIndex)
            {
                // Do a probability check with a likelihood of weights[index] / weightSum.
                if (Random.Range(0, weightSum) < cards[index].weight)
                {
                    return cards[index];
                }

                // Remove the last item from the sum of total untested weights and try again.
                weightSum -= cards[index++].weight;
            }

            // No other item was selected, so return very last index.
            return cards[index];
        }
    }
}
