using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _bodyTransform;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Settings")]
    [SerializeField] private float _movementSpeed = 4f;
    [SerializeField] private float _turningRate = 30f;

    private Vector2 _previousMovementInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) 
            return;

        _inputReader.Move += HandleMove;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
            return;

        _inputReader.Move -= HandleMove;
    }

    private void Update()
    {
        if (!IsOwner) 
            return;

        float zRotation = _previousMovementInput.x * -_turningRate * Time.deltaTime;
        _bodyTransform.Rotate(0f, 0f, zRotation);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        _rb.velocity = (Vector2)_bodyTransform.up * _previousMovementInput.y * _movementSpeed;
    }

    private void HandleMove(Vector2 movementInput)
    {
        _previousMovementInput = movementInput;
    }
}
