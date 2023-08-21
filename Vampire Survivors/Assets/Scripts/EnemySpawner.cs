using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeToSpawn;
    [SerializeField] Transform minSpawn, maxSpawn;
    [SerializeField] int checkPerFrame;
    [SerializeField] List<WaveInfo> waves;

    private float spawnCounter;
    private float despawnDistance;
    private Transform target;
    private List<GameObject> enemies = new List<GameObject>();
    private int enemyToCheck;
    private int currentWave;
    private float waveCounter;

    private void Start()
    {
        //spawnCounter = timeToSpawn;
        currentWave = -1;
        GoToNextWave();

        target = PlayerHealthController.Instance.transform;

        despawnDistance = Vector3.Distance(transform.position, maxSpawn.position) + 4f;
    }

    private void Update()
    {
        //spawnCounter -= Time.deltaTime;
        
        //if (spawnCounter <= 0f)
        //{
        //    spawnCounter = timeToSpawn;

        //    GameObject newEnemy = Instantiate(enemyPrefab, SelectSpawnPoint(), transform.rotation);
        //    enemies.Add(newEnemy);
        //}

        if (PlayerHealthController.Instance.gameObject.activeSelf)
        {
            if (currentWave < waves.Count)
            {
                waveCounter -= Time.deltaTime;
                if (waveCounter <= 0f)
                {
                    GoToNextWave();
                }

                spawnCounter -= Time.deltaTime;
                if (spawnCounter <= 0f)
                {
                    spawnCounter = waves[currentWave].timeBetweenSpawns;

                    GameObject newEnemy = Instantiate(waves[currentWave].enemyToSpawn, SelectSpawnPoint(), Quaternion.identity);

                    enemies.Add(newEnemy);
                }
            }
        }

        transform.position = target.position;

        int checkTarget = enemyToCheck + checkPerFrame;

        while (enemyToCheck < checkTarget)
        {
            if (enemyToCheck < enemies.Count)
            {
                if (enemies[enemyToCheck] != null)
                {
                    if (Vector3.Distance(transform.position, enemies[enemyToCheck].transform.position) > despawnDistance)
                    {
                        Destroy(enemies[enemyToCheck]);

                        enemies.RemoveAt(enemyToCheck);
                        checkTarget--;
                    } else
                    {
                        enemyToCheck++;
                    }
                } else
                {
                    enemies.RemoveAt(enemyToCheck);
                    checkTarget--;
                }
            } else
            {
                enemyToCheck = 0;
                checkTarget = 0;
            }
        }
    }

    private Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = UnityEngine.Random.Range(0f, 1f) > .5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = UnityEngine.Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (UnityEngine.Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            } else
            {
                spawnPoint.x = minSpawn.position.x;
            }
        } else
        {
            spawnPoint.x = UnityEngine.Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (UnityEngine.Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.y = maxSpawn.position.y;
            }
            else
            {
                spawnPoint.y = minSpawn.position.y;
            }
        }

        return spawnPoint;
    }

    private void GoToNextWave()
    {
        currentWave = Math.Min(currentWave + 1, waves.Count - 1);

        waveCounter = waves[currentWave].waveLength;
        spawnCounter = waves[currentWave].timeBetweenSpawns;
    }
}

[Serializable]
public class WaveInfo 
{
    public GameObject enemyToSpawn;
    public float waveLength = 10f;
    public float timeBetweenSpawns = 1f;
}