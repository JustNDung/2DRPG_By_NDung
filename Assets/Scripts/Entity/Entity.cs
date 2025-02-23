using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX entityFX { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CharacterStats characterStats { get; private set; }

    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    public Transform attackCheck;
    public float attackCheckRadius;
    public int facingDirection { get; private set; } = 1;
    protected bool facingRight = true;
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        entityFX = GetComponentInChildren<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterStats = GetComponent<CharacterStats>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void DamageEffect()
    {
        entityFX.StartCoroutine("FlashFX");
        StartCoroutine(HitKnockback());
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }

    #region Velocity    
    public void SetZeroVelocity() {
        if (isKnocked) return;
        rb.linearVelocity = Vector2.zero;
    } 
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion

    #region Collision
    public virtual bool IsGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, groundCheck.position.z));   
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public virtual void FlipController(float x) {
        if (x > 0 && !facingRight) {
            Flip();
        } else if (x < 0 && facingRight) {
            Flip();
        }
    }
    #endregion

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
