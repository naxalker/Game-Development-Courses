using System;
using Unity.Netcode;
using UnityEngine;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();

    [Header("References")]
    [SerializeField] private BountyCoin _coinPrefab;
    [SerializeField] private Health _health;

    [Header("Settings")]
    [SerializeField] private int _bountyCoinCount = 10;
    [SerializeField] private int _minBountyCoinValue = 5;
    [SerializeField] private float _bountyPercentage = 50f;
    [SerializeField] private float _coinSpread = 3f;
    [SerializeField] private LayerMask _layerMask;

    private Collider2D[] _coinBuffer = new Collider2D[1];
    private float _coinRadius;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }
        
        _coinRadius = _coinPrefab.GetComponent<CircleCollider2D>().radius;

        _health.Died += HandleDie;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }

        _health.Died -= HandleDie;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            int coinValue = coin.Collect();

            if (IsServer)
            {
                TotalCoins.Value += coinValue;
            }
        }
    }

    private void HandleDie(Health health)
    {
        int bountyValue = (int)(TotalCoins.Value * (_bountyPercentage / 100f));
        int bountyCoinValue = bountyValue / _bountyCoinCount;

        if (bountyCoinValue < _minBountyCoinValue) { return; }

        for (int i = 0; i < _bountyCoinCount; i++)
        {
            BountyCoin coinInstance = Instantiate(_coinPrefab, GetSpawnPoint(), Quaternion.identity);
            coinInstance.SetValue(bountyCoinValue);
            coinInstance.NetworkObject.Spawn();
        }
    }

    public void SpendCoins(int coinsAmount)
    {
        TotalCoins.Value -= coinsAmount;
    }

    private Vector2 GetSpawnPoint()
    {
        while (true)
        {
            Vector2 spawnPoint = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle * _coinSpread;

            int numColliders = Physics2D.OverlapCircleNonAlloc(spawnPoint, _coinRadius, _coinBuffer, _layerMask);

            if (numColliders == 0)
            {
                return spawnPoint;
            }
        }
    }
}
