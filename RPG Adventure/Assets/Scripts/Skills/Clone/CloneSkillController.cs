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

    private bool _canDuplicateClone;
    private float _chanceToDuplicate;
    private int _facingDir = 1;

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

    public void Initialize(Transform startTransform, float cloneDuration, bool canAttack, Vector3 offset, Transform closestEnemy, bool canDuplicate, float chanceToDuplicate)
    {
        transform.position = startTransform.position + offset;
        _cloneTimer = cloneDuration;
        _closestEnemy = closestEnemy;
        _canDuplicateClone = canDuplicate;
        _chanceToDuplicate = chanceToDuplicate;

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
            {
                hit.GetComponent<Enemy>().DamageEffect();
                
                if (_canDuplicateClone)
                {
                    if (Random.Range(0, 100) < _chanceToDuplicate)
                    {
                        SkillManager.Instance.Clone.CreateClone(hit.transform, new Vector3(1.5f * _facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (_closestEnemy != null)
        {
            if (transform.position.x > _closestEnemy.position.x)
            {
                _facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
