using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(CrossbowVisuals))]
public class TowerCrossbow : Tower
{
    [Header("Crossbow Settings")]
    [SerializeField] private Transform _gunPoint;
    [SerializeField] private int _damage;

    private CrossbowVisuals _crossbowVisuals;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        _crossbowVisuals = GetComponent<CrossbowVisuals>();
        _crossbowVisuals.PlayReloadVFX(AttackCooldown);
    }

    protected override void Attack()
    {
        Vector3 directionToEnemy = GetDirectionToEnemyFrom(_gunPoint);

        if (Physics.Raycast(_gunPoint.position, directionToEnemy, out RaycastHit hit, AttackRange * 2))
        {
            if (hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);

                _crossbowVisuals.PlayReloadVFX(AttackCooldown);
                _crossbowVisuals.PlayAttackVFX(_gunPoint, hit.transform);
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if (CurrentEnemy != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(TowerHead.position, CurrentEnemy.transform.position);
        }
    }
}
