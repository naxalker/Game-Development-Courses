using System;
using Zenject;

public class HealthController : IInitializable, IDisposable
{
    public event Action<int> OnHealthChanged;

    private int _maxHP;
    private int _currentHP;

    private Castle _castle;

    public HealthController(int maxHP, Castle castle)
    {
        _castle = castle;
        _maxHP = maxHP;
    }

    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;

    public void Initialize()
    {
        _currentHP = _maxHP;

        _castle.OnEnemyEntered += EnemyEnteredHandler;
    }

    public void Dispose()
    {
        _castle.OnEnemyEntered -= EnemyEnteredHandler;
    }

    private void EnemyEnteredHandler()
    {
        if (_currentHP <= 0)
            return;

        _currentHP--;

        OnHealthChanged?.Invoke(_currentHP);
    }
}
