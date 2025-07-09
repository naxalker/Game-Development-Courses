using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable
{
    public static event Action<Enemy> OnEnemyDied;

    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private int _healthPoints;

    private NavMeshAgent _agent;
    private int _waypointIndex;
    private float _remainingDistance;
    private Vector3 _previousPosition;
    private Waypoint[] _waypoints;

    public EnemyType EnemyType => _enemyType;
    public float RemainingDistance => _remainingDistance;

    public void Setup(Waypoint[] waypoints, float totalDistance)
    {
        _waypoints = waypoints;
        _remainingDistance = totalDistance +
            Vector3.Distance(transform.position, _waypoints[0].transform.position);
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _agent.updateRotation = false;
        _agent.avoidancePriority = Mathf.RoundToInt(_agent.speed * 10);

        _previousPosition = transform.position;
    }

    private void Update()
    {
        if (_agent.remainingDistance < .25f)
        {
            Vector3 nextWaypoint = GetNextWaypoint();

            _agent.SetDestination(nextWaypoint);
            transform.DOLookAt(nextWaypoint, .5f);
        }

        _remainingDistance -= Vector3.Distance(_previousPosition, transform.position);
        _previousPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        _healthPoints -= damage;

        if (_healthPoints <= 0)
        {
            Die();
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private Vector3 GetNextWaypoint()
    {
        if (_waypointIndex >= _waypoints.Length)
        {
            return transform.position;
        }

        Vector3 targetPoint = _waypoints[_waypointIndex].transform.position;
        _waypointIndex++;

        return targetPoint;
    }

    private void Die()
    {
        OnEnemyDied?.Invoke(this);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
