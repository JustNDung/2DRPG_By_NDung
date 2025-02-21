using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player; 
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        
        return false;
    }

    public virtual void UseSkill()
    {
        // TODO: Do some skill specific things
    }
    
    protected virtual Transform FindClosestEnemy(Transform checkTransform, float attackDetectedRadius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkTransform.position, attackDetectedRadius);
        
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
