using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInput : IInitializable, IDisposable
{
    public event Action InteractPressed;
    public event Action InteractAlternatePressed;

    private PlayerInputActions _inputActions;

    public void Initialize()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

        _inputActions.Player.Interact.performed += InteractPerformedHandler;
        _inputActions.Player.InteractAlternate.performed += InteractAlternatePerformedHandler;
    }

    public void Dispose()
    {
        _inputActions.Player.Interact.performed -= InteractPerformedHandler;
        _inputActions.Player.InteractAlternate.performed -= InteractAlternatePerformedHandler;
    }

    private void InteractPerformedHandler(InputAction.CallbackContext context)
    {
        InteractPressed?.Invoke();
    }

    private void InteractAlternatePerformedHandler(InputAction.CallbackContext context)
    {
        InteractAlternatePressed?.Invoke();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
