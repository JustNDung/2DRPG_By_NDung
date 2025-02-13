using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy_Skeleton;
    private int moveDir;
    public SkeletonBattleState(EnemyStateMachine stateMachine, Enemy enemyBase, string animBoolName, Enemy_Skeleton enemy_Skeleton) 
    : base(stateMachine, enemyBase, animBoolName)
    {
        this.enemy_Skeleton = enemy_Skeleton;
    }

    public override void Enter()
    {
        base.Enter();
        
        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemy_Skeleton.IsPlayerDetected()) {
            if (enemy_Skeleton.IsPlayerDetected().distance < enemy_Skeleton.attackDistance) {
                enemy_Skeleton.ZeroVelocity();
                return;
            }
        } 
        moveDir = player.position.x > enemy_Skeleton.transform.position.x ? 1 : -1;
        enemy_Skeleton.SetVelocity(enemy_Skeleton.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
