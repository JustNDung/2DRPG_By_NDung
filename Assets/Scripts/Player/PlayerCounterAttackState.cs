using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
        
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var detectedObject in detectedObjects) {
            if (detectedObject.TryGetComponent(out Enemy enemy)) {
                if (enemy.CanBeStunned())
                {
                    stateTimer = 10; // any value bigger than 1
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skillManager.clone.CreateCloneOnCounterAttack(detectedObject.transform);
                    }
                }
            }
        }

        if (stateTimer < 0 || triggeredCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
