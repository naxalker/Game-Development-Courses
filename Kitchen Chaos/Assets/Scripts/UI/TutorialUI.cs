using System;
using TMPro;
using UnityEngine;
using Zenject;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _keyMoveUpLabel;
    [SerializeField] private TMP_Text _keyMoveDownLabel;
    [SerializeField] private TMP_Text _keyMoveLeftLabel;
    [SerializeField] private TMP_Text _keyMoveRightLabel;
    [SerializeField] private TMP_Text _keyInteractLabel;
    [SerializeField] private TMP_Text _keyInteractAlternateLabel;
    [SerializeField] private TMP_Text _keyPauseLabel;
    [SerializeField] private TMP_Text _gamepadInteractLabel;
    [SerializeField] private TMP_Text _gamepadInteractAlternateLabel;
    [SerializeField] private TMP_Text _gamepadPauseLabel;
    
    private GameInput _gameInput;
    private GameManager _gameManager;

    [Inject]
    private void Construct(GameInput gameInput, GameManager gameManager)
    {
        _gameInput = gameInput;
        _gameManager = gameManager;
    }

    private void Start()
    {
        UpdateVisual();

        Show();

        _gameInput.BindingRebind += BindingRebindHandler;
        _gameManager.StateChanged += StateChangedHandler;
    }

    

    private void OnDestroy()
    {
        _gameInput.BindingRebind -= BindingRebindHandler;
        _gameManager.StateChanged -= StateChangedHandler;
    }

    private void BindingRebindHandler()
    {
        UpdateVisual();
    }

    private void StateChangedHandler(GameManager.State state)
    {
        if (state == GameManager.State.CountdownToStart)
        {
            Hide();
        }
    }

    private void UpdateVisual()
    {
        _keyMoveUpLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveUp);
        _keyMoveDownLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveDown);
        _keyMoveLeftLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveLeft);
        _keyMoveRightLabel.text = _gameInput.GetBindingText(GameInput.Binding.MoveRight);
        _keyInteractLabel.text = _gameInput.GetBindingText(GameInput.Binding.Interact);
        _keyInteractAlternateLabel.text = _gameInput.GetBindingText(GameInput.Binding.InteractAlternate);
        _keyPauseLabel.text = _gameInput.GetBindingText(GameInput.Binding.Pause);
        _gamepadInteractLabel.text = _gameInput.GetBindingText(GameInput.Binding.GamepadInteract);
        _gamepadInteractAlternateLabel.text = _gameInput.GetBindingText(GameInput.Binding.GamepadInteractAlternate);
        _gamepadPauseLabel.text = _gameInput.GetBindingText(GameInput.Binding.GamepadPause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
