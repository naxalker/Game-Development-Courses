using System;
using Zenject;

public class CurrencyController : IInitializable, IDisposable
{
    public event Action<int> OnCurrencyChanged;
    public event Action OnInsufficientCurrency;

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

    public bool TrySpendCurrency(int amount)
    {
        if (amount <= _currency)
        {
            _currency -= amount;
            OnCurrencyChanged?.Invoke(_currency);

            return true;
        }
        else
        {
            OnInsufficientCurrency?.Invoke();

            return false;
        }
    }

    private void EnemyDiedHandler(Enemy enemy)
    {
        _currency++;

        OnCurrencyChanged?.Invoke(_currency);
    }
}
