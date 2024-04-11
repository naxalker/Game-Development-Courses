using TMPro;
using UnityEngine;
using Zenject;

public class StartCountdownUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _countdownLabel;

    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        _gameManager.StateChanged += StateChangedHandler;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        _countdownLabel.text = Mathf.Ceil(_gameManager.CountdownToStartTimer).ToString();
    }

    private void OnDestroy()
    {
        _gameManager.StateChanged -= StateChangedHandler;
    }

    private void StateChangedHandler(GameManager.State state)
    {
        gameObject.SetActive(state == GameManager.State.CountdownToStart);
    }
}
