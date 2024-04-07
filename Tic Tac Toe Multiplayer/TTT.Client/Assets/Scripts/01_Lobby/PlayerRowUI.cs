using NetworkShared.Packets.ServerClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.Lobby
{
    public class PlayerRowUI : MonoBehaviour
    {
        [SerializeField] private Image _onlineImage, _offlineImage;
        [SerializeField] private TMP_Text _username, _score;
        public void Init(PlayerNetDto player)
        {
            if (player.IsOnline)
            {
                _onlineImage.gameObject.SetActive(true);
            }
            else
            {
                _offlineImage.gameObject.SetActive(true);
            }

            _username.text = player.Username;
            _score.text = player.Score.ToString();
        }
    }
}
