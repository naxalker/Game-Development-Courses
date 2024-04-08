using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInput : IInitializable
{
    public event Action InteractPressed;

    private PlayerInputActions _inputActions;

    public void Initialize()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();

        _inputActions.Player.Interact.performed += InteractPerformedHandler;
    }

    private void InteractPerformedHandler(InputAction.CallbackContext context)
    {
        InteractPressed?.Invoke();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
