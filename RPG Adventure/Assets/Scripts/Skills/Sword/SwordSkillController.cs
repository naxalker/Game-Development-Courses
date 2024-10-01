using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float _returnSpeed = 12f;

    private bool _canRotate = true;
    private bool _isReturning;

    [Header("Bounce Settings")]
    [SerializeField] private float _bounceSpeed = 20f;

    private bool _isBouncing;
    private int _bounceAmount;
    private List<Transform> _enemyTargets = new List<Transform>();
    private int _targetIndex;

    [Header("Pierce Settings")]
    private int _pierceAmount;

    [Header("Spin Settings")]
    private bool _isStopped;
    private bool _isSpinning;
    private float _maxTravelDistance;
    private float _spinDuration;
    private float _spinTimer;
    private float _hitCooldown;
    private float _hitTimer;

    [Header("References")]
    private Animator _animator;
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private Player _player;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_canRotate)
        {
            transform.right = _rb.velocity;
        }

        if (_isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _player.transform.position) < 1)
            {
                _player.CatchTheSword();
            }
        }

        Bounce();

        Spin();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReturning)
            return;

        collision.GetComponent<Enemy>()?.DealDamage();

        if (collision.GetComponent<Enemy>() != null)
        {
            if (_isBouncing && _enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        _enemyTargets.Add(hit.transform);
                }
            }
        }

        StuckInto(collision);
    }

    public void Initialize(Vector2 dir, float gravityScale, Player player)
    {
        _rb.velocity = dir;
        _rb.gravityScale = gravityScale;
        _player = player;

        if (_pierceAmount <= 0)
        {
            _animator.SetBool("Rotation", true);
        }
    }

    public void SetupBounce(bool isBouncing, int amountOfBounces)
    {
        _isBouncing = isBouncing;
        _bounceAmount = amountOfBounces;
    }

    public void SetupPierce(int pierceAmount)
    {
        _pierceAmount = pierceAmount;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        _isSpinning = isSpinning;
        _maxTravelDistance = maxTravelDistance;
        _spinDuration = spinDuration;
        _hitCooldown = hitCooldown;
    }

    public void ReturnSword()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        _isReturning = true;
    }

    private void Bounce()
    {
        if (_isBouncing && _enemyTargets.Count > 0)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, _enemyTargets[_targetIndex].position, _bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _enemyTargets[_targetIndex].position) < 1f)
            {
                _enemyTargets[_targetIndex].GetComponent<Enemy>().DealDamage();

                _targetIndex++;
                _bounceAmount--;

                if (_bounceAmount <= 0)
                {
                    _isBouncing = false;
                    _isReturning = true;
                }

                if (_targetIndex >= _enemyTargets.Count)
                {
                    _targetIndex = 0;
                }
            }
        }
    }

    private void Spin()
    {
        if (_isSpinning)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_isStopped)
            {
                StopWhenSpinning();
            }

            if (_isStopped)
            {
                _spinTimer -= Time.deltaTime;

                if (_spinTimer < 0)
                {
                    _isReturning = true;
                    _isSpinning = false;
                }

                _hitTimer -= Time.deltaTime;

                if (_hitTimer < 0)
                {
                    _hitTimer = _hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            hit.GetComponent<Enemy>().DealDamage();
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        _isStopped = true;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _spinTimer = _spinDuration;
    }

    private void StuckInto(Collider2D collision)
    {
        if (_pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            _pierceAmount--;

            return;
        }

        if (_isSpinning)
        {
            StopWhenSpinning();

            return;
        }

        _canRotate = false;
        _collider.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_isBouncing && _enemyTargets.Count > 0)
            return;

        _animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
