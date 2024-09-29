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
}
