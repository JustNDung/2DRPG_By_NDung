using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        enemy.DamageEffect();
    }

    protected override void Die()
    {
        Debug.Log("Enemy died");
    }
}
