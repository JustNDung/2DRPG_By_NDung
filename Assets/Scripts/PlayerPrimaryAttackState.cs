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
        if (comboCounter > 2 || Time.time > lastTimeAttacked + comboWindow) {
            comboCounter = 0;
        }
        player.anim.SetInteger(comboCounterStr, comboCounter);
    }
    public override void Update()
    {   
        base.Update();

        if (triggeredCalled) {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

}
