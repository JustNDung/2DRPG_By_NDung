using System;
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
            transform.position = Vector2.MoveTowards(
                transform.position, player.transform.position
                , returnSpeed * Time.deltaTime
            );
            if (Vector2.Distance(transform.position, player.transform.position) < distanceToDisappear)
            {
                player.ClearTheSword();
            }
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
        rb.bodyType = RigidbodyType2D.Dynamic;
        transform.parent = null;
        isReturning = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        canRotate = false;
        cd.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = other.transform;
        animator.SetBool(rotation, false);
    }
}
