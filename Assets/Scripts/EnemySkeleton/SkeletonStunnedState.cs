using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private Enemy_Skeleton enemySkeleton;
    public SkeletonStunnedState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton enemySkeleton)
    : base(stateMachine, enemy, animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        enemySkeleton.entityFX.InvokeRepeating("RedColorBlink", 0, 0.1f);
        stateTimer = enemySkeleton.stunDuration;
        rb.linearVelocity = new Vector2(-enemySkeleton.facingDirection *  enemySkeleton.stunDirection.x, enemySkeleton.stunDirection.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) {
            stateMachine.ChangeState(enemySkeleton.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.entityFX.Invoke("CancelColorChange", 0);
    }

    
}
