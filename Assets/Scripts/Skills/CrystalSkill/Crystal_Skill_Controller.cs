using UnityEngine;
using UnityEngine.UIElements;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private Collider2D cd => GetComponent<Collider2D>();
    [SerializeField] private string explode = "Explode";
    private float crystalExistTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    [SerializeField] private float growSpeed;
    [SerializeField] private float distanceFromTargetToCrystal = 1f;
    [SerializeField] private Vector2 maxScale = new Vector2(3, 3);
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    
    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed, Transform closestTarget)
    {
        this.crystalExistTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
        this.closestTarget = closestTarget;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        
        if (colliders.Length > 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }
    
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer <= 0)
        {
            FinishCrystal();
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position
                , closestTarget.position, moveSpeed * Time.deltaTime
            );  
            if (Vector2.Distance(transform.position, closestTarget.position) < distanceFromTargetToCrystal)
            {
                canMove = false;
                FinishCrystal();
            }
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, maxScale, growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(
            transform.position
            , cd.bounds.size.x
        );   
        foreach (var detectedObject in detectedObjects)
        {
            if (detectedObject.TryGetComponent(out Enemy enemy))
            {
                enemy.Damage();
            }
        }
    }
    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true; 
            anim.SetTrigger(explode);
        }
        else
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
