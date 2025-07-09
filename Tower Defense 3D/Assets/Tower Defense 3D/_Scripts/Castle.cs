using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Castle : MonoBehaviour
{
    public event Action OnEnemyEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Enemy enemy))
        {
            enemy.DestroyEnemy();

            OnEnemyEntered?.Invoke();
        }
    }
}
