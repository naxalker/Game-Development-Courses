using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInput : IInitializable, IDisposable
{
    private const string BindingsPlayerPrefs = "InputBindings";

    public event Action InteractPressed;
    public event Action InteractAlternatePressed;
    public event Action PausePressed;
    public event Action BindingRebind;

    public enum Binding
    {
        MoveUp,
        MoveDown,
        MoveLeft,
        MoveRight,
        Interact,
        InteractAlternate,
        Pause,
        GamepadInteract,
        GamepadInteractAlternate,
        GamepadPause
    }

    private PlayerInputActions _inputActions;

    public void Initialize()
    {
        _inputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(BindingsPlayerPrefs))
        {
            _inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(BindingsPlayerPrefs));
        }

        _inputActions.Enable();

        _inputActions.Player.Interact.performed += InteractPerformedHandler;
        _inputActions.Player.InteractAlternate.performed += InteractAlternatePerformedHandler;
        _inputActions.Player.Pause.performed += PausePerformedHandler;
    }

    public void Dispose()
    {
        _inputActions.Player.Interact.performed -= InteractPerformedHandler;
        _inputActions.Player.InteractAlternate.performed -= InteractAlternatePerformedHandler;
        _inputActions.Player.Pause.performed += PausePerformedHandler;

        _inputActions.Dispose();
    }

    private void InteractPerformedHandler(InputAction.CallbackContext context)
    {
        InteractPressed?.Invoke();
    }

    private void InteractAlternatePerformedHandler(InputAction.CallbackContext context)
    {
        InteractAlternatePressed?.Invoke();
    }

    private void PausePerformedHandler(InputAction.CallbackContext context)
    {
        PausePressed?.Invoke();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:

            case Binding.MoveUp:
                return _inputActions.Player.Move.bindings[1].ToDisplayString();

            case Binding.MoveDown:
                return _inputActions.Player.Move.bindings[2].ToDisplayString();

            case Binding.MoveLeft:
                return _inputActions.Player.Move.bindings[3].ToDisplayString();

            case Binding.MoveRight:
                return _inputActions.Player.Move.bindings[4].ToDisplayString();

            case Binding.Interact:
                return _inputActions.Player.Interact.bindings[0].ToDisplayString();

            case Binding.InteractAlternate:
                return _inputActions.Player.InteractAlternate.bindings[0].ToDisplayString();

            case Binding.Pause:
                return _inputActions.Player.Pause.bindings[0].ToDisplayString();

            case Binding.GamepadInteract:
                return _inputActions.Player.Interact.bindings[1].ToDisplayString();

            case Binding.GamepadInteractAlternate:
                return _inputActions.Player.InteractAlternate.bindings[1].ToDisplayString();

            case Binding.GamepadPause:
                return _inputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        _inputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:

            case Binding.MoveUp:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 1;
                break;

            case Binding.MoveDown:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 2;
                break;

            case Binding.MoveLeft:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 3;
                break;

            case Binding.MoveRight:
                inputAction = _inputActions.Player.Move;
                bindingIndex = 4;
                break;

            case Binding.Interact:
                inputAction = _inputActions.Player.Interact;
                bindingIndex = 0;
                break;

            case Binding.InteractAlternate:
                inputAction = _inputActions.Player.InteractAlternate;
                bindingIndex = 0;
                break;

            case Binding.Pause:
                inputAction = _inputActions.Player.Pause;
                bindingIndex = 0;
                break;

            case Binding.GamepadInteract:
                inputAction = _inputActions.Player.Interact;
                bindingIndex = 1;
                break;

            case Binding.GamepadInteractAlternate:
                inputAction = _inputActions.Player.InteractAlternate;
                bindingIndex = 1;
                break;

            case Binding.GamepadPause:
                inputAction = _inputActions.Player.Pause;
                bindingIndex = 1;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                _inputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(BindingsPlayerPrefs, _inputActions.SaveBindingOverridesAsJson());

                BindingRebind?.Invoke();
            })
            .Start();
    }
}
