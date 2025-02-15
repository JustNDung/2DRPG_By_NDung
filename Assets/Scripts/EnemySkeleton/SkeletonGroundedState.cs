using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemySkeleton;
    protected Transform player;
    public SkeletonGroundedState(EnemyStateMachine stateMachine, Enemy enemy, string animBoolName, Enemy_Skeleton enemySkeleton) 
    : base(stateMachine, enemy, animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemySkeleton.IsPlayerDetected() || Vector2.Distance(player.transform.position, enemySkeleton.transform.position) < 2) {
            stateMachine.ChangeState(enemySkeleton.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

