using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class TankPlayer : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera _followCamera;

    [Header("Settings")]
    [SerializeField] private int _ownerPriority = 15;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            _followCamera.Priority = _ownerPriority;
        }
    }
}
