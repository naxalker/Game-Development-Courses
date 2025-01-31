using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsEnemy;

    private float _crystalExistTimer;

    private bool _canExplode;

    private bool _canMove;
    private float _moveSpeed;

    private bool _canGrow;
    private float _growSpeed = 5f;

    private Transform _closestEnemy;

    private Animator _animator;
    private CircleCollider2D _circleCollider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        _crystalExistTimer -= Time.deltaTime;

        if (_crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (_canGrow)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(3, 3), _growSpeed * Time.deltaTime);
        }

        if (_canMove)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, _closestEnemy.position, _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _closestEnemy.position) < 1f)
            {
                FinishCrystal();
            }
        }
    }

    public void Initialize(float crystalDuration, bool canExplode, bool canMove, float moveSpeed, Transform closestEnemy)
    {
        _crystalExistTimer = crystalDuration;
        _canExplode = canExplode;
        _canMove = canMove;
        _moveSpeed = moveSpeed;
        _closestEnemy = closestEnemy;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.Instance.Blackhole.GetblackholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length > 0)
        {
            _closestEnemy = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }

    public void FinishCrystal()
    {
        _canMove = false;

        if (_canExplode)
        {
            _canGrow = true;
            _animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestroy();
        }
    }

    private void SelfDestroy() => Destroy(gameObject);

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _circleCollider.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().DamageEffect();
        }
    }
}
