using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [Header("Transforms")]
    [SerializeField] protected Transform TowerHead;
    [SerializeField] protected float RotationSpeed;

    [Header("Attack Settings")]
    [SerializeField] protected float AttackRange;
    [SerializeField] protected float AttackCooldown;
    [SerializeField] protected EnemyType PriorityEnemyType;
    [SerializeField] protected bool DynamicTargetChange = true;

    protected Enemy CurrentEnemy;
    protected float LastTimeAttacked;
    protected bool CanRotate = true;

    private float _lastChangeTime;

    protected bool CanAttack =>
        Time.time > LastTimeAttacked + AttackCooldown &&
        CurrentEnemy != null;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        if (CurrentEnemy == null)
        {
            CurrentEnemy = FindNextEnemyWithRange();
        }
        else if (DynamicTargetChange && Time.time > _lastChangeTime + 1f)
        {
            CurrentEnemy = FindNextEnemyWithRange();
        }
        else if (Vector3.Distance(transform.position, CurrentEnemy.transform.position) > AttackRange)
        {
            CurrentEnemy = null;
        }

        if (CanAttack)
        {
            Attack();
            LastTimeAttacked = Time.time;
        }

        RotateTowardsEnemy();
    }

    protected abstract void Attack();

    protected virtual void RotateTowardsEnemy()
    {
        if (CurrentEnemy == null || CanRotate == false) { return; }

        Vector3 direction = CurrentEnemy.transform.position - TowerHead.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        TowerHead.rotation = Quaternion.Slerp(
            TowerHead.rotation,
            lookRotation,
            RotationSpeed * Time.deltaTime
        );
    }

    protected Enemy FindNextEnemyWithRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            AttackRange,
            LayerMask.GetMask("Enemy")
        );

        Enemy enemyWithPriority = hitColliders
            .Select(collider => collider.GetComponent<Enemy>())
            .Where(enemy => enemy != null)
            .OrderBy(enemy => enemy.EnemyType == PriorityEnemyType ? 0 : 1)
            .ThenBy(enemy => enemy.RemainingDistance)
            .FirstOrDefault();

        return enemyWithPriority;
    }

    protected Vector3 GetDirectionToEnemyFrom(Transform startPoint)
    {
        if (CurrentEnemy == null) { return Vector3.zero; }

        Vector3 direction = CurrentEnemy.transform.position + new Vector3(0, .25f, 0) - startPoint.position;
        return direction.normalized;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
