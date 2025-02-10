using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) 
    : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Additional logic for entering the wall slide state
    }

    public override void Update()
    {
        base.Update();
        if (xInput != 0) {
            if (player.facingDirection != xInput) {
                stateMachine.ChangeState(player.idleState);
            }
        }
        if (player.IsGroundedDetected()) {
            stateMachine.ChangeState(player.idleState);
        }
        if (yInput < 0) {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * 0.7f);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // Additional logic for exiting the wall slide state
    }
}
