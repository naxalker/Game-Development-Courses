using System;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameManager : IDisposable
{
    private const string MenuSceneName = "Menu";

    private JoinAllocation _allocation;
    private NetworkClient _networkClient;
    private MatchplayMatchmaker _matchmaker;
    private UserData _userData;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();

        _networkClient = new NetworkClient(NetworkManager.Singleton);
        _matchmaker = new MatchplayMatchmaker();

        AuthState authState = await AuthenticationWrapper.DoAuth();

        if (authState == AuthState.Authenticated)
        {
            _userData = new UserData
            {
                UserAuthId = AuthenticationService.Instance.PlayerId,
                UserName = PlayerPrefs.GetString(NameSelector.PlayerNameKey, "Missing Name")
            };

            return true;
        }

        return false;
    }

    public void Dispose()
    {
        _networkClient?.Dispose();
    }

    public void Disconnect()
    {
        _networkClient.Disconnect();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }

    public void StartClient(string ip, int port)
    {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(ip, (ushort)port);

        ConnectClient();
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            _allocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        RelayServerData relayServerData = new RelayServerData(_allocation, "dtls");
        transport.SetRelayServerData(relayServerData);

        ConnectClient();
    }

    public async void MatchmakeAsync(Action<MatchmakerPollingResult> onMatchmakeResponse)
    {
        if (_matchmaker.IsMatchmaking)
        {
            return;
        }

        MatchmakerPollingResult matchResult = await GetMatchAsync();
        onMatchmakeResponse?.Invoke(matchResult);
    }

    public async Task CancelMatchmaking()
    {
        await _matchmaker.CancelMatchmaking();
    }

    private void ConnectClient()
    {
        string payload = JsonUtility.ToJson(_userData);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;

        NetworkManager.Singleton.StartClient();
    }

    private async Task<MatchmakerPollingResult> GetMatchAsync()
    {
        MatchmakingResult matchmakingResult = await _matchmaker.Matchmake(_userData);

        if (matchmakingResult.result == MatchmakerPollingResult.Success)
        {
            StartClient(matchmakingResult.ip, matchmakingResult.port);
        }

        return matchmakingResult.result;
    }
}
