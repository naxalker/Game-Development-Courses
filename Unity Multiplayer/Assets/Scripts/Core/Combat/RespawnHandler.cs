using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private TankPlayer _playerPrefab;
    [SerializeField, Range(0, 100)] private float _keptCoinPercentage;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; }

        TankPlayer[] players = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);
        foreach (TankPlayer player in players)
        {
            HandlePlayerSpawned(player);
        }

        TankPlayer.Spawned += HandlePlayerSpawned;
        TankPlayer.Despawned += HandlePlayerDespawned;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) { return; }

        TankPlayer.Spawned -= HandlePlayerSpawned;
        TankPlayer.Despawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(TankPlayer player)
    {
        player.Health.Died += (health) => HandlePlayerDie(player);
    }

    private void HandlePlayerDespawned(TankPlayer player)
    {
        player.Health.Died -= (health) => HandlePlayerDie(player);
    }

    private void HandlePlayerDie(TankPlayer player)
    {
        int keptCoins = (int)(player.Wallet.TotalCoins.Value * (_keptCoinPercentage / 100));

        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId, keptCoins));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId, int keptCoins)
    {
        yield return null;

        TankPlayer playerInstance =
            Instantiate(_playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        
        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        playerInstance.Wallet.TotalCoins.Value += keptCoins;
    }
}
