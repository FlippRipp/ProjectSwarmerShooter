using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using  System.Linq;
using Random = UnityEngine.Random;

namespace FG
{

    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private ObjectPooler.ObjectType[] enemyTypes;
        [SerializeField] private int maxEnemiesAlive = 200;
        [SerializeField] private int waveSize = 20;
        [SerializeField] private SpawnPoint[] spawnPoints;
        [SerializeField] private Transform target;
        [SerializeField] private float bossSpawnPercentage = 2;
        [SerializeField] private int enemiesStartAmount = 100;
        [SerializeField] private float enemiesPerSecond = 1;
        private readonly List<SwarmerEnemy> aliveEnemies = new List<SwarmerEnemy>();

        public static EnemyManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private bool spawningEnemies;

        private void Update()
        {
            if (aliveEnemies.Count <  Mathf.Min(enemiesStartAmount + enemiesPerSecond * Time.time, maxEnemiesAlive) && !spawningEnemies)
            {
                spawningEnemies = true;
                StartCoroutine(SpawnEnemies());
            }
        }

        private void OnDeath(SwarmerEnemy enemy)
        {
            aliveEnemies.Remove(enemy);
            enemy.OnDeath -= OnDeath;
        }

        private SpawnPoint FindSpawnPoint()
        {
            for (int i = 0; i < 10; i++)
            {

                int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
                if (!spawnPoints[randomSpawnPoint].isVisible)
                {
                    return spawnPoints[randomSpawnPoint];
                }
            }
            int randomBackUpSpawnPoint = Random.Range(0, spawnPoints.Length);
            return spawnPoints[randomBackUpSpawnPoint];
        }

        private ObjectPooler.ObjectType GetEnemyType()
        {
            float random = Random.Range(0f, 100f);

            if (random < 100 - bossSpawnPercentage)
            {
                return enemyTypes[0];
            }

            return enemyTypes[1];
        }

        private IEnumerator SpawnEnemies()
        {
            SpawnPoint spawnPoint = FindSpawnPoint();
            for (int i = 0; i < waveSize; i++)
            {
                Vector3 spawnPointPos = spawnPoint.transform.position;

                float randomPosX = Random.Range(spawnPointPos.x - spawnPoint.size.x / 2,
                    spawnPointPos.x + spawnPoint.size.x / 2);

                float randomPosZ = Random.Range(spawnPointPos.z - spawnPoint.size.y / 2,
                    spawnPointPos.z + spawnPoint.size.y / 2);

                Vector3 pos = new Vector3(randomPosX, spawnPointPos.y, randomPosZ);

                GameObject enemy = ObjectPooler.instance.GetPooledObject(GetEnemyType());
                if (enemy)
                {
                    enemy.transform.position = pos;
                    SwarmerEnemy swarmerEnemy = enemy.GetComponent<SwarmerEnemy>();
                    if (swarmerEnemy)
                    {
                        aliveEnemies.Add(swarmerEnemy);
                        swarmerEnemy.OnDeath += OnDeath;
                        swarmerEnemy.target = target;
                    }
                }
                yield return null;
            }

            spawningEnemies = false;
        }

        public void Explosion(Vector3 pos, float force, float radius)
        {
            List<SwarmerEnemy> nearbyEnemies = aliveEnemies.Where( e => (e.transform.position - pos).sqrMagnitude < radius * radius ).ToList();
            foreach (SwarmerEnemy enemy in nearbyEnemies)
            {
                enemy.Death();
                enemy.body.AddExplosionForce(force, pos, radius);
            }
        }
    }
}
