using System;
using UnityEngine;

public class Player : MonoBehaviour
{   
    public float moveSpeed = 12f;
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    [SerializeField] private String idle = "Idle";
    [SerializeField] private String move = "Move";
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, idle);
        moveState = new PlayerMoveState(this, stateMachine, move);
    }
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.currentState.Update();
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
    }

}
