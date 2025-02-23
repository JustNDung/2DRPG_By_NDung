using System;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private float returnSpeed = 12;
    [SerializeField] private float distanceToDisappear = 1f;
    [SerializeField] private string rotation = "Rotate";
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private float freezeTimeDuration;

    [Header("Pierce info")]
    [SerializeField] private int amountOfPierce;
    
    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int amountOfBounce;
    public List<Transform> enemyTarget;
    private int targetIndex;
    
    [Header("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    [SerializeField] private float existingTime = 7f;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    
    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.linearVelocity;
        }

        if (isReturning)
        {
            ReturningLogic();
        }

        if (isBouncing && enemyTarget.Count > 0)
        {
            BouncingLogic();
        }

        if (isSpinning)
        {
            SpinningLogic();
        }
        
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void SpinningLogic()
    {
        if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
        {
            StopWhenSpinning();
            // Stop di chuyển khi đi quá xa
        }

        if (wasStopped)
        {
            spinTimer -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position
                , new Vector2(transform.position.x + spinDirection, transform.position.y)
                , 1.5f * Time.deltaTime
            );
            // Di chuyển a little bit theo hướng đã quay
            if (spinTimer < 0)
            {
                isReturning = true;
                isSpinning = false;
            }
        }

        hitTimer -= Time.deltaTime;
        if (hitTimer < 0)
        {
            hitTimer = hitCooldown;
                
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

            foreach (var hit in colliders)
            {
                if (hit.TryGetComponent(out Enemy enemy))
                {
                    SwordSkillDamage(enemy);
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BouncingLogic()
    {
        transform.position = Vector2.MoveTowards(transform.position
            , enemyTarget[targetIndex].position
            , bounceSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
        {
            SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());;
            targetIndex++;
            amountOfBounce--;
            if (amountOfBounce <= 0)
            {
                isBouncing = false;
                isReturning = true;
            }
            
            if (targetIndex >= enemyTarget.Count)
            {
                targetIndex = 0; 
            }
        }
    }

    private void ReturningLogic()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, player.transform.position
            , returnSpeed * Time.deltaTime
        );
        animator.SetBool(rotation, true);
        if (Vector2.Distance(transform.position, player.transform.position) < distanceToDisappear)
        {
            player.CatchSword();
        }
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)
    {
        this.returnSpeed = returnSpeed;
        this.player = player;
        this.freezeTimeDuration = freezeTimeDuration;
        rb.linearVelocity = dir;
        rb.gravityScale = gravityScale;

        if (amountOfPierce <= 0)
        {
            animator.SetBool(rotation, true);
        }
        
        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1); 
        Invoke("DestroyMe", existingTime);
    }
    
    public void SetupBounce(bool isBouncing, int amountOfBounce, float bounceSpeed)
    {
        this.bounceSpeed = bounceSpeed;
        this.isBouncing = isBouncing;
        this.amountOfBounce = amountOfBounce;
        enemyTarget = new List<Transform>();
    }

    public void SetupPierce(int amountOfPierce)
    {
        this.amountOfPierce = amountOfPierce;
    }
    
    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        this.isSpinning = isSpinning;
        this.maxTravelDistance = maxTravelDistance;
        this.spinDuration = spinDuration;
        this.hitCooldown = hitCooldown;
    }

    public void ReturnSword()
    {
        //rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
        {
            return;
        }

        if (other.TryGetComponent(out Enemy enemy))
        {
            SwordSkillDamage(enemy);
        }
        
        SetupTargetsForBounce(other);
        
        StuckInto(other);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.DamageEffect();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    private void SetupTargetsForBounce(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.TryGetComponent(out Enemy enemy))
                    {
                        enemyTarget.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInto(Collider2D other)
    {
        if (amountOfPierce > 0 && other.GetComponent<Enemy>() != null)
        {
            amountOfPierce--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }
        
        canRotate = false;
        cd.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
        {
            return;
        }
        transform.parent = other.transform;
        animator.SetBool(rotation, false);
    }
}
