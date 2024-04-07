using UnityEngine;

public class Player : MonoBehaviour
{
    private const float PlayerRadius = .7f;
    private const float PlayerHeight = 2f;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotateSpeed = 10f;

    [Header("References")]
    [SerializeField] private GameInput _gameInput;

    private Rigidbody _rb;
    private bool _isWalking;

    public bool IsWalking => _isWalking;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        _rb.velocity = moveDir * _moveSpeed;

        _isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, _rotateSpeed * Time.deltaTime);
    }
}
