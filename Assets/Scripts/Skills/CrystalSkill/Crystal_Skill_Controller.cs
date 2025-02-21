using UnityEngine;

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
    
    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed)
    {
        this.crystalExistTimer = crystalDuration;
        this.canExplode = canExplode;
        this.canMove = canMove;
        this.moveSpeed = moveSpeed;
    }
    
    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer <= 0)
        {
            FinishCrystal();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] detectedObjects =
            Physics2D.OverlapCircleAll(transform.position, cd.bounds.size.x);
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
