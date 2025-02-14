using System;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    [SerializeField] private String comboCounterStr = "ComboCounter";
    private int comboCounter;
    private float lastTimeAttacked;
    [SerializeField] private float comboWindow = 2;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
    : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0;
        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow) {
            comboCounter = 0;
        }
        player.anim.SetInteger(comboCounterStr, comboCounter);

        float attackDir = player.facingDirection;
        if (xInput != 0) {
            attackDir = xInput;
        }
        
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
        stateTimer = 0.1f;
    }
    public override void Update()
    {   
        base.Update();

        if (stateTimer < 0) {
            player.SetZeroVelocity();
        }

        if (triggeredCalled) {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.2f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

}
