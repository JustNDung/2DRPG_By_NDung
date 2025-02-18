using System;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    [SerializeField] private float distanceToDisappear = 1f;
    [SerializeField] private string rotation = "Rotate";
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    public float bounceSpeed = 20f;
    public bool isBouncing = true;
    public int amountOfBounce = 4;
    public List<Transform> enemyTarget;
    private int targetIndex;
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
    }

    private void BouncingLogic()
    {
        transform.position = Vector2.MoveTowards(transform.position
            , enemyTarget[targetIndex].position
            , bounceSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
        {
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

    public void SetupSword(Vector2 dir, float gravityScale, Player player)
    {
        this.player = player;
        rb.linearVelocity = dir;
        rb.gravityScale = gravityScale;

        animator.SetBool(rotation, true);
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
        
        StuckInto(other);
    }

    private void StuckInto(Collider2D other)
    {
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
