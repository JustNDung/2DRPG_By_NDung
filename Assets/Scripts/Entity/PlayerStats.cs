using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        
        player = GetComponent<Player>();
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
