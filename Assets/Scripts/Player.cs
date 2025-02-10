using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{   
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce = 16f;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDirection { get; private set; } = 1;
    private bool facingRight = true;

    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    [SerializeField] private String idle = "Idle";
    [SerializeField] private String move = "Move"; 
    [SerializeField] private String jump = "Jump";
    [SerializeField] private String dash = "Dash";
    [SerializeField] private String wallSlide = "WallSlide";

    #endregion
    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, idle);
        moveState = new PlayerMoveState(this, stateMachine, move);
        jumpState = new PlayerJumpState(this, stateMachine, jump);
        airState = new PlayerAirState(this, stateMachine, jump);
        dashState = new PlayerDashState(this, stateMachine, dash);
        wallSlideState = new PlayerWallSlideState(this, stateMachine, wallSlide);
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
        CheckForDashInput();
    }

    private void CheckForDashInput() {
        dashTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer < 0) {
            dashTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0) {
                dashDir = facingDirection;
            }
            stateMachine.ChangeState(dashState);
        }
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }

    public bool IsGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, groundCheck.position.z));   
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
    }

    public void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void FlipController(float x) {
        if (x > 0 && !facingRight) {
            Flip();
        } else if (x < 0 && facingRight) {
            Flip();
        }
    }

}
