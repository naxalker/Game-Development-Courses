using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animator))]
public class StartCountdownUI : MonoBehaviour
{
    private const string NumberPopup = "NumberPopup";

    [SerializeField] private TMP_Text _countdownLabel;

    private GameManager _gameManager;
    private SoundManager _soundManager;
    private Animator _animator;

    private int _previousCountdownNumber;

    [Inject]
    private void Construct(GameManager gameManager, SoundManager soundManager)
    {
        _gameManager = gameManager;
        _soundManager = soundManager;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gameManager.StateChanged += StateChangedHandler;

        gameObject.SetActive(false);
    }

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(_gameManager.CountdownToStartTimer);
        _countdownLabel.text = countdownNumber.ToString();

        if (_previousCountdownNumber != countdownNumber)
        {
            _previousCountdownNumber = countdownNumber;
            _animator.SetTrigger(NumberPopup);
            _soundManager.PlayCountdownSound();
        }
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
