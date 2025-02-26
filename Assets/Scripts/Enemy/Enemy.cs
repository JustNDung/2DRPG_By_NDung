using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;
    
    [Header("Attack info")]
    public float attackCooldown;
    public float lastTimeAttacked;
    public float attackDistance;
    public EnemyStateMachine stateMachine { get; private set; } 
    public string lastAnimBoolName { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();   
    }
    
    public override void SlowEntityBy(float slowPercentage, float duration)
    {
        base.SlowEntityBy(slowPercentage, duration);
        moveSpeed = moveSpeed * (1 - slowPercentage);
        anim.speed = anim.speed * (1 - slowPercentage);
        
        Invoke("ReturnDefaultSpeed", duration);
    }
    
    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void AssignLastAnimName(string animBoolName)
    {
        lastAnimBoolName = animBoolName;
    }

    public virtual void FreezeTime(bool timeFrozen)
    {
        if (timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(seconds);
        FreezeTime(false);
    }

    #region Counter attack

    public virtual void OpenCounterAttackWindow() {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow() {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    #endregion

    public virtual bool CanBeStunned() {
        if (canBeStunned) {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() 
    => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            transform.position, 
            new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y)
        );
    }
}
