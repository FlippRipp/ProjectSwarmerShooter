using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FG
{

    public class EnemyWaves : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemyPrefabs;
        [SerializeField] private int maxEnemiesAlive = 200;
        [SerializeField] private int waveSize = 20;
        [SerializeField] private SpawnPoint[] spawnPoints;
        [SerializeField] private Transform target;
        [SerializeField] private float bossSpawnPercentage = 2;
        [SerializeField] private int enemiesStartAmount = 100;
        [SerializeField] private int enemiesPerSecond = 1;
        private List<GameObject> aliveEnemies = new List<GameObject>();

        private bool spawningEnemies;

        private void Update()
        {
            if (aliveEnemies.Count <  Mathf.Min(enemiesStartAmount + enemiesPerSecond * Time.time, maxEnemiesAlive) && !spawningEnemies)
            {
                spawningEnemies = true;
                StartCoroutine(SpawnEnemies());
            }
        }

        private void DeathEnemy(GameObject enemy)
        {
            aliveEnemies.Remove(enemy);
        }

        private SpawnPoint FindSpawnPoint()
        {
            while (true)
            {
                int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
                if (!spawnPoints[randomSpawnPoint].isVisible)
                {
                    return spawnPoints[randomSpawnPoint];
                }
            }
        }

        private GameObject GetEnemyType()
        {
            float random = Random.Range(0f, 100f);

            if (random < 100 - bossSpawnPercentage)
            {
                return enemyPrefabs[0];
            }

            return enemyPrefabs[1];
        }

        private IEnumerator SpawnEnemies()
        {
            SpawnPoint spawnPoint = FindSpawnPoint();
            for (int i = 0; i < waveSize; i++)
            {
                Vector3 spawnPointPos = spawnPoint.transform.position;

                float randomPosX = Random.Range(spawnPointPos.x - spawnPoint.size.x / 2,
                    spawnPoint.position.x + spawnPoint.size.x / 2);

                float randomPosZ = Random.Range(spawnPointPos.z - spawnPoint.size.y / 2,
                    spawnPoint.position.z + spawnPoint.size.y / 2);

                Vector3 pos = new Vector3(randomPosX, spawnPointPos.y, randomPosZ);
                
                GameObject enemy = Instantiate(GetEnemyType(), spawnPointPos, Quaternion.identity);
                aliveEnemies.Add(enemy);
                SwarmerEnemy swarmerEnemy = enemy.GetComponent<SwarmerEnemy>();
                swarmerEnemy.EnemyDeath += DeathEnemy;
                swarmerEnemy.target = target;
                yield return null;
            }

            spawningEnemies = false;
        }
    }
}
