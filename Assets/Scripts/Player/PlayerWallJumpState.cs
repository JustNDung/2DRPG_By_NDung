using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float timer = .4f;
    private float wallJumpForce = 5f;
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
    : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = timer;
        player.SetVelocity(wallJumpForce * -player.facingDirection, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0) {
            stateMachine.ChangeState(player.airState);
        }
        if (player.IsGroundedDetected()) {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
