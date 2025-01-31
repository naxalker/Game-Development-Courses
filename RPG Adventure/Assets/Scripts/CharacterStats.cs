using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat Strength;
    public Stat Damage;
    public Stat MaxHealth;

    private int _currentHealth;

    protected virtual void Start()
    {
        _currentHealth = MaxHealth.Value;
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        int totalDamage = Damage.Value + Strength.Value;

        targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
    }
}
