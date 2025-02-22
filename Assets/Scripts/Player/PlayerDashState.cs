using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
    : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skillManager.clone.CreateCloneOnDashStart();
        // player.skillManager.clone.CreateClone(player.transform, Vector3.zero);
        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        if (!player.IsGroundedDetected() && player.IsWallDetected()) {
            stateMachine.ChangeState(player.wallSlideState);
        }
        if (stateTimer < 0) {
            stateMachine.ChangeState(player.idleState);
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        player.skillManager.clone.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.linearVelocity.y);
    }

    
}
