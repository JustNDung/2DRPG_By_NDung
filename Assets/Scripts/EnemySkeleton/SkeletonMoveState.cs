using UnityEngine;

public class SkeletonMoveState : EnemyState
{
    private Enemy_Skeleton enemySkeleton;
    public SkeletonMoveState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton enemySkeleton)
    : base(stateMachine, enemy, animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
 
        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDirection, enemySkeleton.rb.linearVelocity.y);
        
        if (enemySkeleton.IsWallDetected() || !enemySkeleton.IsGroundedDetected()) {
            enemySkeleton.Flip();
            stateMachine.ChangeState(enemySkeleton.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
