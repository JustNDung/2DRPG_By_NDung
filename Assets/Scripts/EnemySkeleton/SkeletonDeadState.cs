
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private Enemy_Skeleton enemySkeleton;
    public SkeletonDeadState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemySkeleton) 
        : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        enemySkeleton.anim.SetBool(enemySkeleton.lastAnimBoolName, true);
        enemySkeleton.anim.speed = 0;
        enemySkeleton.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            rb.linearVelocity = new Vector2(0, 10);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
