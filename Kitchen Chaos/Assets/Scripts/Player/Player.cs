using System;
using UnityEngine;
using Zenject;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event Action<BaseCounter> SelectedCounterChanged;
    public event Action PickedSomething;

    private const float InteractDistance = 2f;

    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private float _rotateSpeed = 10f;

    [Header("References")]
    [SerializeField] private Transform _kitchenObjectHoldPoint;

    private KitchenObject _kitchenObject;

    private bool _isWalking;

    private GameInput _gameInput;
    private GameManager _gameManager;
    private Rigidbody _rb;
    private BaseCounter _selectedCounter;

    [Inject]
    private void Construct(GameInput gameInput, GameManager gameManager)
    {
        _gameInput = gameInput;
        _gameManager = gameManager;
    }

    public bool IsWalking => _isWalking;

    public Transform KitchenObjectFollowTransform => _kitchenObjectHoldPoint;

    public KitchenObject KitchenObject
    {
        get { return _kitchenObject; }
        set
        {
            _kitchenObject = value;

            if (_kitchenObject != null)
            {
                PickedSomething?.Invoke();
            }
        }
    }

    public bool HasKitchenObject => _kitchenObject != null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _gameInput.InteractPressed += InteractPressedHandler;
        _gameInput.InteractAlternatePressed += InteractAlternatePressedHandler;
    }

    private void OnDisable()
    {
        _gameInput.InteractPressed -= InteractPressedHandler;
        _gameInput.InteractAlternatePressed -= InteractAlternatePressedHandler;
    }

    private void Update()
    {
        HandleInteractions();
        HandleMovement();
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    private void HandleInteractions()
    {
        int counterLayer = LayerMask.NameToLayer("Counters");
        int onlyCountersLayerMask = 1 << counterLayer;

        BaseCounter baseCounter = null;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, InteractDistance, onlyCountersLayerMask))
        {
            baseCounter = raycastHit.transform.GetComponent<BaseCounter>();
        }

        if (baseCounter != _selectedCounter)
        {
            _selectedCounter = baseCounter;

            SelectedCounterChanged?.Invoke(_selectedCounter);
        }
    }

    private void InteractPressedHandler()
    {
        if (_gameManager.IsGamePlaying == false) return;

        _selectedCounter?.Interact(this);
    }

    private void InteractAlternatePressedHandler()
    {
        if (_gameManager.IsGamePlaying == false) return;

        _selectedCounter?.InteractAlternate(this);
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
