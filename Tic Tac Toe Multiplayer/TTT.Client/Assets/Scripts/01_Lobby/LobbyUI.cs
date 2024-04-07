using NetworkShared.Packets.ClientServer;
using NetworkShared.Packets.ServerClient;
using System;
using TMPro;
using TTT.PacketHandlers;
using UnityEngine;

namespace TTT.Lobby
{
    public class LobbyUI : MonoBehaviour
    {
        [SerializeField] private PlayerRowUI _playerRowPrefab;

        private TMP_Text _playersOnlineLabel;
        private Transform _topPlayersContainer;

        private void Start()
        {
            _topPlayersContainer = transform.Find("Top Players Container");
            _playersOnlineLabel = transform.Find("Players Online").GetComponent<TMP_Text>();

            RequestServerStatus();

            OnServerStatusRequestHandler.OnServerStatus += Refresh;
        }

        private void OnDestroy()
        {
            OnServerStatusRequestHandler.OnServerStatus -= Refresh;
        }

        private void RequestServerStatus()
        {
            var msg = new Net_ServerStatusRequest();
            NetworkClient.Instance.SendServer(msg);
        }

        private void Refresh(Net_OnServerStatus msg)
        {
            while (_topPlayersContainer.childCount > 0)
            {
                DestroyImmediate(_topPlayersContainer.GetChild(0).gameObject);
            }

            _playersOnlineLabel.text = $"{msg.PlayersCount} players online";

            for (int i = 0; i < msg.TopPlayers.Length; i++)
            {
                var player = msg.TopPlayers[i];
                var instance = Instantiate(_playerRowPrefab, _topPlayersContainer);
                instance.Init(player);
            }
        }

        // FindOpponent()

        // CancelFindOpponent()

        // Logout()

        // RefreshUI()
    }
}
