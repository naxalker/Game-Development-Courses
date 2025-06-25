using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private MobileJoystick joystick;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        Vector3 correctedJoystickVector = joystick.move;
        correctedJoystickVector.z = correctedJoystickVector.y;
        correctedJoystickVector.y = 0;

        playerAnimator.ManageAnimations(correctedJoystickVector);

        Vector3 moveVector = joystick.move * moveSpeed * Time.deltaTime;

        moveVector.z = moveVector.y;
        moveVector.y = -1;

        characterController.Move(moveVector);


    }
}
