using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkServer : IDisposable
{
    public event Action<string> OnClientLeft;
    public event Action<UserData> OnUserJoined;
    public event Action<UserData> OnUserLeft;

    private NetworkManager _networkManager;
    private NetworkObject _playerPrefab;

    private Dictionary<ulong, string> _clientIdToAuth = new Dictionary<ulong, string>();
    private Dictionary<string, UserData> _authIdToUserData = new Dictionary<string, UserData>();

    public NetworkServer(NetworkManager networkManager, NetworkObject playerPrefab)
    {
        _networkManager = networkManager;
        _playerPrefab = playerPrefab;

        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
        _networkManager.OnServerStarted += OnNetworkReady;
    }

    public void Dispose()
    {
        if (_networkManager != null)
        {
            _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
            _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
            _networkManager.OnServerStarted -= OnNetworkReady;
        }
    }

    public UserData GetUserDataByClientId(ulong clientId)
    {
        if (_clientIdToAuth.TryGetValue(clientId, out string authId))
        {
            if (_authIdToUserData.TryGetValue(authId, out UserData userData))
            {
                return userData;
            }
        }

        return null;
    }

    public bool OpenConnection(string ip, int port)
    {
        UnityTransport transport = _networkManager.gameObject.GetComponent<UnityTransport>();
        transport.SetConnectionData(ip, (ushort)port);
        
        return _networkManager.StartServer();
    }

    private void ApprovalCheck(
        NetworkManager.ConnectionApprovalRequest request,
        NetworkManager.ConnectionApprovalResponse response)
    {
        string payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(payload);

        _clientIdToAuth[request.ClientNetworkId] = userData.UserAuthId;
        _authIdToUserData[userData.UserAuthId] = userData;

        OnUserJoined?.Invoke(userData);

        _ = SpawnPlayerDelayed(request.ClientNetworkId);

        response.Approved = true;
        response.CreatePlayerObject = false;
    }

    private async Task SpawnPlayerDelayed(ulong clientId)
    {
        await Task.Delay(1000);

        NetworkObject playerInstance =
            GameObject.Instantiate(_playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        playerInstance.SpawnAsPlayerObject(clientId);
    }

    private void OnNetworkReady()
    {
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (_clientIdToAuth.TryGetValue(clientId, out string authId))
        {
            _clientIdToAuth.Remove(clientId);

            OnUserLeft?.Invoke(_authIdToUserData[authId]);

            _authIdToUserData.Remove(authId);

            OnClientLeft?.Invoke(authId);
        }
    }
}
