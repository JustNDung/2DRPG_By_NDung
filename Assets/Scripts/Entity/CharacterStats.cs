using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float damage;
    public float maxHp;
    
    private float currentHp;
    void Start()
    {
        currentHp = maxHp;
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
