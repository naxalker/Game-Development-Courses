using System;
using Zenject;

public class CurrencyController : IInitializable, IDisposable
{
    public event Action<int> OnCurrencyChanged;

    private int _currency = 500;

    public int Currency => _currency;

    public void AddCurrency(int amount)
    {
        _currency += amount;
    }

    public void Initialize()
    {
        Enemy.OnEnemyDied += EnemyDiedHandler;
    }

    public void Dispose()
    {
        Enemy.OnEnemyDied -= EnemyDiedHandler;
    }

    public void SpendCurrency(int amount)
    {
        if (amount <= _currency)
        {
            _currency -= amount;
            OnCurrencyChanged?.Invoke(_currency);
        }
    }

    private void EnemyDiedHandler(Enemy enemy)
    {
        _currency++;

        OnCurrencyChanged?.Invoke(_currency);
    }
}
