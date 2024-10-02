using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private float _colorLoosingSpeed;
    [SerializeField] private Transform _attackCheck;
    [SerializeField] private float _attackCheckRadius = .8f;

    private float _cloneTimer;
    private Transform _closestEnemy;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _cloneTimer -= Time.deltaTime;

        if (_cloneTimer <= 0)
        {
            _spriteRenderer.color = new Color(1, 1, 1, _spriteRenderer.color.a - (Time.deltaTime * _colorLoosingSpeed));

            if (_spriteRenderer.color.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Initialize(Transform startTransform, float cloneDuration, bool canAttack, Vector3 offset)
    {
        transform.position = startTransform.position + offset;
        _cloneTimer = cloneDuration;

        if (canAttack)
        {
            _animator.SetInteger("AttackNumber", Random.Range(1, 4));
        }

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        _cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheck.position, _attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().DealDamage();
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    _closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        if (_closestEnemy != null)
        {
            if (transform.position.x > _closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
