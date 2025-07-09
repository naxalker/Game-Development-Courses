using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

public class EnemyPortal : MonoBehaviour
{
    [Header("Wave Configuration")]
    [SerializeField]
    private WaveConfig[] _waveConfigs;

    [Header("Waypoints")]
    [SerializeField] private Waypoint[] _waypoints;
    [ShowNonSerializedField] private float _totalDistance;

    private float _spawnTimer;
    private Queue<EnemyType> _enemiesToSpawn = new Queue<EnemyType>();
    private int _waveIndex = 0;

    private EnemyFactory _enemyFactory;

    [Inject]
    private void Construct(EnemyFactory enemyFactory)
    {
        _enemyFactory = enemyFactory;
    }

    private void Start()
    {
        CalculateTotalDistance();

        _spawnTimer = 0f;
    }

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f && _enemiesToSpawn.Count != 0)
        {
            SpawnEnemy();
            _spawnTimer = _waveConfigs[_waveIndex].EnemySpawnInterval;
        }
    }

    public int StartNewWave(int waveIndex)
    {
        if (waveIndex >= _waveConfigs.Length)
        {
            Debug.Log($"All waves at {name} completed!");
            return 0;
        }

        _waveIndex = waveIndex;
        Queue<EnemyType> newWave = CreateNewWave();

        while (newWave.Count > 0)
        {
            _enemiesToSpawn.Enqueue(newWave.Dequeue());
        }

        return _enemiesToSpawn.Count;
    }

    private void SpawnEnemy()
    {
        EnemyType enemyType = _enemiesToSpawn.Dequeue();
        Enemy enemy = _enemyFactory.CreateEnemy(enemyType, transform.position);
        enemy.Setup(_waypoints, _totalDistance);
    }

    private Queue<EnemyType> CreateNewWave()
    {
        List<EnemyType> newWave = new List<EnemyType>();

        foreach (var kvp in _waveConfigs[_waveIndex].EnemiesCount)
        {
            newWave.AddRange(Enumerable.Repeat(kvp.Key, kvp.Value));
        }

        if (_waveConfigs[_waveIndex].RandomizeWave)
        {
            var random = new System.Random();
            newWave = newWave.OrderBy(x => random.Next()).ToList();
        }

        return new Queue<EnemyType>(newWave);
    }

    private void CalculateTotalDistance()
    {
        _totalDistance = 0f;
        for (int i = 0; i < _waypoints.Length - 1; i++)
        {
            Vector3 a = _waypoints[i].transform.position;
            Vector3 b = _waypoints[i + 1].transform.position;
            a.y = 0f;
            b.y = 0f;
            _totalDistance += Vector3.Distance(a, b);
        }
    }

#if UNITY_EDITOR
    [Button("Collect Waypoints")]
    private void CollectWaypoints()
    {
        _waypoints = new List<Waypoint>(GetComponentsInChildren<Waypoint>()).ToArray();

        CalculateTotalDistance();
    }
#endif
}
