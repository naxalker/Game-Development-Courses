using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    public event Action<ClearCounter> SelectedCounterChanged;
    
    private const float InteractDistance = 2f;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotateSpeed = 10f;

    private bool _isWalking;

    private GameInput _gameInput;
    private Rigidbody _rb;
    private ClearCounter _selectedCounter;

    public bool IsWalking => _isWalking;

    [Inject]
    private void Construct(GameInput gameInput)
    {
        _gameInput = gameInput;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _gameInput.InteractPressed += InteractPressedHandler;
    }

    private void OnDisable()
    {
        _gameInput.InteractPressed -= InteractPressedHandler;
    }

    private void Update()
    {
        HandleInteractions();
        HandleMovement();
    }

    private void HandleInteractions()
    {
        int counterLayer = LayerMask.NameToLayer("Counters");
        int onlyCountersLayerMask = 1 << counterLayer;

        ClearCounter clearCounter = null;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, InteractDistance, onlyCountersLayerMask))
        {
            clearCounter = raycastHit.transform.GetComponent<ClearCounter>();
        }

        if (clearCounter != _selectedCounter)
        {
            _selectedCounter = clearCounter;

            SelectedCounterChanged?.Invoke(_selectedCounter);
        }
    }

    private void InteractPressedHandler()
    {
        _selectedCounter?.Interact();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        _rb.velocity = moveDir * _moveSpeed;

        _isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, _rotateSpeed * Time.deltaTime);
    }
}
