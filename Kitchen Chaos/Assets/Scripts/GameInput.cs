using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _inputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        return inputVector;
    }
}
