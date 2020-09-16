using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaves : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemiesAlive = 200;
    [SerializeField] private int waveSize = 20;
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private Transform target;
    private List<GameObject> aliveEnemies = new List<GameObject>();

    private bool spawningEnemies;

    private void Start()
    {
    }

    private void Update()
    {
        if (aliveEnemies.Count < maxEnemiesAlive && !spawningEnemies)
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
            
            GameObject enemy = Instantiate(enemyPrefab, spawnPointPos, Quaternion.identity);
            aliveEnemies.Add(enemy);
            SwarmerEnemy swarmerEnemy = enemy.GetComponent<SwarmerEnemy>();
            swarmerEnemy.EnemyDeath += DeathEnemy;
            swarmerEnemy.target = target;
            yield return null;
        }

        spawningEnemies = false;
    }
}
