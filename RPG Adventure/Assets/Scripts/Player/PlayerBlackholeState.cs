using UnityEngine;

public class PlayerBlackholeState : PlayerState
{
    private bool _skillUsed;
    private float _flyTime = .4f;
    private float _defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _defaultGravity = player.rb.gravityScale;

        _skillUsed = false;
        stateTimer = _flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
        }
        else
        {
            rb.velocity = new Vector2(0, -.1f);

            if (!_skillUsed)
            {
                if (player.SkillManagerInstance.Blackhole.CanUseSkill)
                {
                    player.SkillManagerInstance.Blackhole.UseSkill();
                    _skillUsed = true;
                }
            }
        }

        if (player.SkillManagerInstance.Blackhole.SkillCompleted())
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = _defaultGravity;
        player.MakeTransparent(false);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
