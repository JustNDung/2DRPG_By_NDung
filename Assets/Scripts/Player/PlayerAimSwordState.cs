using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skillManager.sword.DotsActive(true);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idleState);
        }
        Flip();
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
    }

    private void Flip()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (player.transform.position.x > mousePosition.x && player.facingDirection == 1)
        {
            player.Flip();
        } 
        else if (player.transform.position.x < mousePosition.x && player.facingDirection == -1)
        {
            player.Flip();
        }
    }
}
