using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat maxHp;
    public Stat strength;
    
    [SerializeField] private float currentHp;
    protected virtual void Start()
    {
        currentHp = maxHp.GetBaseValue();
    }

    public virtual void DoDamage(CharacterStats target)
    {
        float totalDamage = damage.GetBaseValue() + strength.GetBaseValue();
        target.TakeDamage(totalDamage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public virtual void TakeDamage(float damage)
    {
        currentHp -= damage;
        
        if (currentHp <= 0)
        {
            Die();
        }
        
        Debug.Log("Take damage: " + damage);
    }
    
    protected virtual void Die()
    {
        
    }
}
