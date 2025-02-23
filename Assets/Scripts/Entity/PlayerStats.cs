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

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        player.DamageEffect();
    }

    protected override void Die()
    {
        Debug.Log("Player died");
    }
}
