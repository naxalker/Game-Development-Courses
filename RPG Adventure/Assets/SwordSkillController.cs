using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private CircleCollider2D _collider;
    private Player _player;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
    }

    public void Initialize(Vector2 dir, float gravityScale)
    {
        _rb.velocity = dir;
        _rb.gravityScale = gravityScale;
    }
}
