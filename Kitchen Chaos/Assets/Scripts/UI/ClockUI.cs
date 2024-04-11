using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private Image _clockImage;

    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Update()
    {
        _clockImage.fillAmount = _gameManager.GamePlayingTimerNormalized;
    }
}
