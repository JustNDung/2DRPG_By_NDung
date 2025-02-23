using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;
    private Color originalColor;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    private Transform closestEnemy;
    
    private bool canDuplicateClone;
    private int facingDirection = 1;
    private float createCloneRate;

    [SerializeField] private string attackNumber = "AttackNumber";
    [SerializeField] private int minAttackNumber = 1;
    [SerializeField] private int maxAttackNumber = 3;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - colorLosingSpeed * Time.deltaTime);
        }
    }
    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack
        , Vector3 offset, Transform closestEnemy, bool canDuplicateClone, float createClonerate)
    {
        if (canAttack)
        {
            anim.SetInteger(attackNumber, Random.Range(minAttackNumber, maxAttackNumber));
        }
        spriteRenderer.color = originalColor;
        transform.position = newTransform.position + offset;
        cloneTimer = cloneDuration;
        this.closestEnemy = closestEnemy;
        this.canDuplicateClone = canDuplicateClone;
        this.createCloneRate = createClonerate;
        
        FaceClosestTarget();
    }
    
    private void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    private void AttackTrigger()    
    {
        Collider2D[] detectedObjects =
            Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var detectedObject in detectedObjects)
        {
            if (detectedObject.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage();
                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < createCloneRate)
                    {
                        SkillManager.instance.clone.CreateClone(detectedObject.transform, new Vector3(.5f * facingDirection, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
