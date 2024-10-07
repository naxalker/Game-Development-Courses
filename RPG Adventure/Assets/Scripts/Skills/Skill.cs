using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float Cooldown;

    protected float CooldownTimer;
    protected Player Player;

    protected virtual void Start()
    {
        Player = PlayerManager.Instance.Player;
    }

    protected virtual void Update()
    {
        CooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill => CooldownTimer <= 0;

    public virtual void UseSkill()
    {
        if (CanUseSkill)
        {
            CooldownTimer = Cooldown;
        }
    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        return closestEnemy;
    }
}
