using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button _soundFXButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _moveUpButton;
    [SerializeField] private Button _moveDownButton;
    [SerializeField] private Button _moveLeftButton;
    [SerializeField] private Button _moveRightButton;
    [SerializeField] private Button _interactButton;
    [SerializeField] private Button _interactAlternateButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _gamepadInteractButton;
    [SerializeField] private Button _gamepadInteractAlternateButton;
    [SerializeField] private Button _gamepadPauseButton;

    [SerializeField] private TMP_Text _soundFXText;
    [SerializeField] private TMP_Text _musicText;
    [SerializeField] private TMP_Text _moveUpLabel;
    [SerializeField] private TMP_Text _moveDownLabel;
    [SerializeField] private TMP_Text _moveLeftLabel;
    [SerializeField] private TMP_Text _moveRightLabel;
    [SerializeField] private TMP_Text _interactLabel;
    [SerializeField] private TMP_Text _interactAlternateLabel;
    [SerializeField] private TMP_Text _pauseLabel;
    [SerializeField] private TMP_Text _gamepadInteractLabel;
    [SerializeField] private TMP_Text _gamepadInteractAlternateLabel;
    [SerializeField] private TMP_Text _gamepadPauseLabel;

    [SerializeField] private GameObject _pressToRebindKeyPanel;

    private SoundManager _soundManager;
    private MusicManager _musicManager;
    private GameManager _gameManager;
    private GameInput _gameInput;

    private Action _onCloseButtonAction;

    [Inject]
    private void Construct(SoundManager soundManager, MusicManager musicManager, GameManager gameManager, GameInput gameInput)
    {
        _soundManager = soundManager;
        _musicManager = musicManager;
        _gameManager = gameManager;
        _gameInput = gameInput;
    }

    private void Awake()
    {
        _soundFXButton.onClick.AddListener(() =>
        {
            _soundManager.ChangeVolume();
            UpdateVisual();
        });

        _musicButton.onClick.AddListener(() =>
        {
            _musicManager.ChangeVolume();
            UpdateVisual();
        });

        _closeButton.onClick.AddListener(() =>
        {
            Hide();
            _onCloseButtonAction();
        });

        _moveUpButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveUp); });
        _moveDownButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveDown); });
        _moveLeftButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveLeft); });
        _moveRightButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveRight); });
        _interactButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact); });
        _interactAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate); });
        _pauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
        _gamepadInteractButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamepadInteract); });
        _gamepadInteractAlternateButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamepadInteractAlternate); });
        _gamepadPauseButton.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamepadPause); });
    }

    private void Start()
    {
        _gameManager.GameUnpaused += GameUnpausedHandler;

        UpdateVisual();

        _pressToRebindKeyPanel.SetActive(false);
        Hide();
    }

    private void OnDestroy()
    {
        _gameManager.GameUnpaused -= GameUnpausedHandler;
    }

    public void Show(Action onCloseButtonAction)
    {
        _onCloseButtonAction = onCloseButtonAction;

        gameObject.SetActive(true);

        _soundFXButton.Select();
    }

    private void UpdateVisual()
    {
        _soundFXText.text = "Sound Effects: " + Mathf.Round(_soundManager.Volume * 10f);
        _musicText.text = "Music: " + Mathf.Round(_musicManager.Volume * 10f);

        _moveUpLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveUp);
        _moveDownLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveDown);
        _moveLeftLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveLeft);
        _moveRightLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveRight);
        _interactLabel.text = _gameInput.GetBindingText(GameInput.Binding.Interact);
        _interactAlternateLabel.text = _gameInput.GetBindingText(GameInput.Binding.InteractAlternate);
        _pauseLabel.text = _gameInput.GetBindingText(GameInput.Binding.Pause);
        _gamepadInteractLabel.text = _gameInput.GetBindingText(GameInput.Binding.GamepadInteract);
        _gamepadInteractAlternateLabel.text = _gameInput.GetBindingText(GameInput.Binding.GamepadInteractAlternate);
        _gamepadPauseLabel.text = _gameInput.GetBindingText(GameInput.Binding.GamepadPause);
    }

    private void GameUnpausedHandler()
    {
        Hide();
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        _pressToRebindKeyPanel.SetActive(true);
        _gameInput.RebindBinding(binding, () =>
        {
            _pressToRebindKeyPanel.SetActive(false);
            UpdateVisual();
        });
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
