using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private Enemy_Skeleton enemySkeleton;
    public SkeletonAttackState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton enemy_Skeleton) 
    : base(stateMachine, enemy, animBoolName)
    {
        this.enemySkeleton = enemy_Skeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        enemySkeleton.SetZeroVelocity();
        if (triggerCalled) {
            stateMachine.ChangeState(enemySkeleton.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemySkeleton.lastTimeAttacked = Time.time;
    }
}
