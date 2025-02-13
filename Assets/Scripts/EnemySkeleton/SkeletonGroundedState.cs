using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemySkeleton;
    public SkeletonGroundedState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton enemySkeleton) 
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
        if (enemySkeleton.IsPlayerDetected()) {
            stateMachine.ChangeState(enemySkeleton.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

