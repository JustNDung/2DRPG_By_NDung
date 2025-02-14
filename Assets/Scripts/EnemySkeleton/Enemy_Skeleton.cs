using System;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }    
    [SerializeField] private string idle = "Idle";
    [SerializeField] private string move = "Move";
    [SerializeField] private string attack = "Attack";

    #endregion
    private Vector2 velocity;
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(stateMachine, this, idle, this);
        moveState = new SkeletonMoveState(stateMachine, this, move, this);
        battleState = new SkeletonBattleState(stateMachine, this, move, this);
        attackState = new SkeletonAttackState(stateMachine, this, attack, this);
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
    }
}
