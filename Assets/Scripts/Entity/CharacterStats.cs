using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat maxHp;
    
    [SerializeField] private float currentHp;
    void Start()
    {
        currentHp = maxHp.GetBaseValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        
        if (currentHp <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
    }
}
