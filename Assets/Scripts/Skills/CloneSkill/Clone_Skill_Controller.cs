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
    [SerializeField] private float attackDetectedRadius = 25f;
    private Transform closestEnemy;

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
    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack)
    {
        if (canAttack)
        {
            anim.SetInteger(attackNumber, Random.Range(minAttackNumber, maxAttackNumber));
        }
        spriteRenderer.color = originalColor;
        transform.position = newTransform.position;
        cloneTimer = cloneDuration;
        
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
            }
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackDetectedRadius);
        
        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
