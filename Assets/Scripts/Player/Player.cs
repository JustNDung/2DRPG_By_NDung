using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public GameObject sword; 
    public SkillManager skillManager;
    [Header("Attack details")]
    public Vector2[] attackMovement;

    public float counterAttackDuration = 0.2f;
    public bool isBusy {get; private set;}

    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce = 16f;
    public float swordReturnImpact = 10f;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    
    [SerializeField] private String idle = "Idle";
    [SerializeField] private String move = "Move"; 
    [SerializeField] private String jump = "Jump";
    [SerializeField] private String dash = "Dash";
    [SerializeField] private String wallSlide = "WallSlide";
    [SerializeField] private String primaryAttack = "Attack";
    [SerializeField] private String counterAttack = "CounterAttack";
    [SerializeField] private String aimSword = "AimSword";
    [SerializeField] private String catchSword = "CatchSword";

    #endregion
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, idle);
        moveState = new PlayerMoveState(this, stateMachine, move);
        jumpState = new PlayerJumpState(this, stateMachine, jump);
        airState = new PlayerAirState(this, stateMachine, jump);
        dashState = new PlayerDashState(this, stateMachine, dash);
        wallSlideState = new PlayerWallSlideState(this, stateMachine, wallSlide);
        wallJumpState = new PlayerWallJumpState(this, stateMachine, jump);

        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, primaryAttack);
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, counterAttack);
        
        aimSwordState = new PlayerAimSwordState(this, stateMachine, aimSword);
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, catchSword);
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, jump);
    }
    protected override void Start()
    {
        base.Start();
        skillManager = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    public void ExitBlackHole()
    {
        stateMachine.ChangeState(airState);
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    public IEnumerator BusyFor(float duration) {
        isBusy = true;
        yield return new WaitForSeconds(duration);
        isBusy = false;
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput() {
        if (IsWallDetected()) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill()) {
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0) {
                dashDir = facingDirection;
            }
            stateMachine.ChangeState(dashState);
        }
    }
}
