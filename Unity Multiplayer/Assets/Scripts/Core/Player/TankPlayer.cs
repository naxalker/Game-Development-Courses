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
    [SerializeField] private Texture2D _crosshair;

    [Header("Settings")]
    [SerializeField] private int _ownerPriority = 15;
    [SerializeField] private Color _minimapColor;

    public Health Health => _health;
    public CoinWallet Wallet => _wallet;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = null;

            if (IsHost)
            {
                userData = HostSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            }
            else
            {
                userData = ServerSingleton.Instance.GameManager.NetworkServer.GetUserDataByClientId(OwnerClientId);
            }

            PlayerName.Value = userData.UserName;

            Spawned?.Invoke(this);
        }

        if (IsOwner)
        {
            _followCamera.Priority = _ownerPriority;

            _minimapIconRenderer.color = _minimapColor;

            Cursor.SetCursor(_crosshair, new Vector2(_crosshair.width / 2, _crosshair.height / 2), CursorMode.Auto);
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
