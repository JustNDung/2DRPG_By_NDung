using System;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }    
    public SkeletonStunnedState stunnedState { get; private set; }
    [SerializeField] private string idle = "Idle";
    [SerializeField] private string move = "Move";
    [SerializeField] private string attack = "Attack";
    [SerializeField] private string stunned = "Stunned";

    #endregion
    private Vector2 velocity;
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(stateMachine, this, idle, this);
        moveState = new SkeletonMoveState(stateMachine, this, move, this);
        battleState = new SkeletonBattleState(stateMachine, this, move, this);
        attackState = new SkeletonAttackState(stateMachine, this, attack, this);
        stunnedState = new SkeletonStunnedState(stateMachine, this, stunned, this);
    }

    protected override void Start()
    {
        base.Start(); 
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        velocity = rb.linearVelocity;

        if (Input.GetKeyDown(KeyCode.U)) {
            stateMachine.ChangeState(stunnedState);
        }
    }

    protected override bool CanBeStunned()
    {
        if (base.CanBeStunned()) {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}
