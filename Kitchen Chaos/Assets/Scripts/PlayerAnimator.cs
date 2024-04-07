using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IsWalking = "IsWalking";

    [SerializeField] private Player _player;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(IsWalking, _player.IsWalking);
    }
}
