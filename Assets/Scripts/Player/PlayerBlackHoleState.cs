using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .4f;
    private bool skillUsed;
    private float defaultGravityScale;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
    : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravityScale = rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 15);
        }
        else
        {
            if (!skillUsed)
            {
                if (player.skillManager.blackHole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
            rb.linearVelocity = new Vector2(0, -0.1f);
        }

        if (player.skillManager.blackHole.BlackHoleFinshed())
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravityScale;
        PlayerManager.instance.player.entityFX.MakeTransparent(false);
    }
}

