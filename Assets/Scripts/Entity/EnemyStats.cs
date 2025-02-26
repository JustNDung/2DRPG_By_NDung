
public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>(); 
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
