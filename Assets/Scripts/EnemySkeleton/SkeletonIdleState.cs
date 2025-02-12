using UnityEngine;

public class SkeletonIdleState : EnemyState
{
    private Enemy_Skeleton enemySkeleton;
    public SkeletonIdleState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton enemySkeleton) 
    : base(stateMachine, enemy, animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemySkeleton.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) {
            stateMachine.ChangeState(enemySkeleton.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
