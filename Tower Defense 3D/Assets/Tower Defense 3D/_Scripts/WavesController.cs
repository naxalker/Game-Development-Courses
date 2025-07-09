using System;
using UnityEngine;
using Zenject;

public class WavesController : IInitializable, IDisposable, ITickable
{
    public event Action OnFinalWaveStarted;
    public event Action<int> OnWaveStarted;
    public event Action<int> OnWaveCompleted;

    private int _waveIndex = 0;
    private int _totalEnemiesInWave = 0;
    private EnemyPortal[] _enemyPortals;
    private float _waveTimer = 0f;

    private readonly int[] _wavesDuration;

    public WavesController(int[] wavesDuration)
    {
        _wavesDuration = wavesDuration;
    }

    public float WaveTimer => _waveTimer;

    public void Initialize()
    {
        _enemyPortals = UnityEngine.Object.FindObjectsByType<EnemyPortal>(FindObjectsSortMode.None);

        StartNewWave();

        Enemy.OnEnemyDied += EnemyDiedHandler;
    }

    public void Dispose()
    {
        Enemy.OnEnemyDied += EnemyDiedHandler;
    }

    public void Tick()
    {
        if (_waveIndex < _wavesDuration.Length)
        {
            _waveTimer -= Time.deltaTime;

            if (_waveTimer <= 0f)
            {
                Debug.Log($"Wave {_waveIndex} timed out!");
                StartNewWave();
            }
        }
    }

    public void ForceNextWave()
    {
        if (_waveIndex < _wavesDuration.Length)
        {
            StartNewWave();
        }
    }

    private void StartNewWave()
    {
        if (_waveIndex == _wavesDuration.Length - 1)
        {
            OnFinalWaveStarted?.Invoke();
        }
        else
        {
            OnWaveStarted?.Invoke(_waveIndex);
        }

        _waveTimer = _wavesDuration[_waveIndex];

        foreach (EnemyPortal portal in _enemyPortals)
        {
            _totalEnemiesInWave += portal.StartNewWave(_waveIndex);
        }

        _waveIndex++;
    }

    private void EnemyDiedHandler(Enemy enemy)
    {
        _totalEnemiesInWave--;

        if (_totalEnemiesInWave <= 0)
        {
            OnWaveCompleted?.Invoke(_waveIndex - 1);
        }
    }
}
