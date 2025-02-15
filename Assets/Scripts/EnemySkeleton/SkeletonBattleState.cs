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
        
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        if (enemy_Skeleton.IsPlayerDetected()) {
            if (enemy_Skeleton.IsPlayerDetected().distance < enemy_Skeleton.attackDistance) {
                stateTimer = enemy_Skeleton.battleTime;
                if (CanAttack()) {
                    stateMachine.ChangeState(enemy_Skeleton.attackState);
                }
            }
        } else {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy_Skeleton.transform.position) > 10) {
                stateMachine.ChangeState(enemy_Skeleton.idleState);
            }
        }
        moveDir = player.position.x > enemy_Skeleton.transform.position.x ? 1 : -1;
        enemy_Skeleton.SetVelocity(enemy_Skeleton.moveSpeed * moveDir, rb.linearVelocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack() {
        if (Time.time >= enemy_Skeleton.lastTimeAttacked + enemy_Skeleton.attackCooldown) {
            enemy_Skeleton.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }

}
