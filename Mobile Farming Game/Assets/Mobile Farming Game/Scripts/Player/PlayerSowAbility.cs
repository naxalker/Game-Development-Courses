using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerSowAbility : MonoBehaviour
{
    [Header("Elements")]
    private PlayerAnimator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField"))
            playerAnimator.PlaySowAnimation();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
            playerAnimator.StopSowAnimation();
    }
}
