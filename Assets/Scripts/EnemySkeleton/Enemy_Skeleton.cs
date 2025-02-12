using System;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    [SerializeField] private string idle = "Idle";
    [SerializeField] private string move = "Move";

    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(stateMachine, this, idle, this);
        moveState = new SkeletonMoveState(stateMachine, this, move, this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
