using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using Unity.Collections;
using System;

public class TankPlayer : NetworkBehaviour
{
    public static event Action<TankPlayer> Spawned;
    public static event Action<TankPlayer> Despawned;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();

    [Header("References")]
    [SerializeField] private Health _health;
    [SerializeField] private CoinWallet _wallet;
    [SerializeField] private CinemachineVirtualCamera _followCamera;
    [SerializeField] private SpriteRenderer _minimapIconRenderer;

    [Header("Settings")]
    [SerializeField] private int _ownerPriority = 15;
    [SerializeField] private Color _minimapColor;

    public Health Health => _health;
    public CoinWallet Wallet => _wallet;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData =
                HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);

            PlayerName.Value = userData.UserName;

            Spawned?.Invoke(this);
        }

        if (IsOwner)
        {
            _followCamera.Priority = _ownerPriority;
            _minimapIconRenderer.color = _minimapColor;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            Despawned?.Invoke(this);
        }
    }
}
