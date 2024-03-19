using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class ServerGameManager : IDisposable
{
    private string _serverIP;
    private int _serverPort;
    private int _queryPort;
    private NetworkServer _networkServer;
    private MatchplayBackfiller _backfiller;
    private MultiplayAllocationService _multiplayAllocationService;

    public ServerGameManager(string serverIP, int serverPort, int queryPort, NetworkManager manager, NetworkObject playerPrefab)
    {
        _serverIP = serverIP;
        _serverPort = serverPort;
        _queryPort = queryPort;
        _networkServer = new NetworkServer(manager, playerPrefab);
        _multiplayAllocationService = new MultiplayAllocationService();
    }

    public void Dispose()
    {
        _multiplayAllocationService?.Dispose();
        _networkServer?.Dispose();
        _backfiller?.Dispose();

        _networkServer.OnUserJoined -= UserJoined;
        _networkServer.OnUserLeft -= UserLeft;
    }

    public NetworkServer NetworkServer => _networkServer;

    public async Task StartGameServerAsync()
    {
        await _multiplayAllocationService.BeginServerCheck();

        try
        {
            MatchmakingResults matchmakingPayload = await GetMatchmakerPayload();

            if (matchmakingPayload != null)
            {
                await StartBackfill(matchmakingPayload);
                _networkServer.OnUserJoined += UserJoined;
                _networkServer.OnUserLeft += UserLeft;
            }
            else
            {
                Debug.LogWarning("Matchmaker payload timed out");
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

        if (_networkServer.OpenConnection(_serverIP, _serverPort) == false)
        {
            Debug.LogWarning("Network Server did not start as expected.");
            return;
        }
    }

    private async Task<MatchmakingResults> GetMatchmakerPayload()
    {
        Task<MatchmakingResults> matchmakerPayloadTask =
            _multiplayAllocationService.SubscribeAndAwaitMatchmakerAllocation();

        if (await Task.WhenAny(matchmakerPayloadTask, Task.Delay(20_000)) == matchmakerPayloadTask)
        {
            return matchmakerPayloadTask.Result;
        }

        return null;
    }

    private async Task StartBackfill(MatchmakingResults payload)
    {
        _backfiller = new MatchplayBackfiller(
            $"{_serverIP}:{_serverPort}",
            payload.QueueName,
            payload.MatchProperties,
            20);

        if (_backfiller.NeedsPlayers())
        {
            await _backfiller.BeginBackfilling();
        }
    }

    private void UserJoined(UserData user)
    {
        _backfiller.AddPlayerToMatch(user);
        _multiplayAllocationService.AddPlayer();
        
        if (_backfiller.NeedsPlayers() == false && _backfiller.IsBackfilling)
        {
            _ = _backfiller.StopBackfill();
        }
    }

    private void UserLeft(UserData user)
    {
        int playerCount = _backfiller.RemovePlayerFromMatch(user.UserAuthId);
        _multiplayAllocationService.RemovePlayer();

        if (playerCount <= 0)
        {
            CloseServer();
            return;
        }

        if (_backfiller.NeedsPlayers() && _backfiller.IsBackfilling == false)
        {
            _ = _backfiller.BeginBackfilling();
        }
    }

    private async void CloseServer()
    {
        await _backfiller.StopBackfill();
        Dispose();
        Application.Quit();
    }
}
