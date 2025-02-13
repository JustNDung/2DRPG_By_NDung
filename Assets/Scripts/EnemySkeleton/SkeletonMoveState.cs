using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton enemySkeleton)
    : base(stateMachine, enemy, animBoolName, enemySkeleton)
    {
    }

    public override void Enter()
    {
        base.Enter(); 
    } 

    public override void Update()
    {
        base.Update();
 
        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDirection, rb.linearVelocity.y);
        
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
