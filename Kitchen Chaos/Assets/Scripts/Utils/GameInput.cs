using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInput : IInitializable, IDisposable
{
    public event Action InteractPressed;
    public event Action InteractAlternatePressed;
    public event Action PausePressed;

    private PlayerInputActions _inputActions;

    public void Initialize()
    {
        _inputActions = new PlayerInputActions();
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
}
