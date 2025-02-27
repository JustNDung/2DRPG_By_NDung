
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;

    [Header("Level details")] [SerializeField] private int level;

    [Range(0f, 1f)] [SerializeField] private float percentageModifier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();
        enemy = GetComponent<Enemy>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(damage);
        Modify(maxHp);
        Modify(armor);
        Modify(magicResistance);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);
        Modify(critChance);
        Modify(critPower);
        Modify(evasion);
        Modify(strength);
        
        Modify(iceDamage);
        Modify(lightningDamage);
        Modify(fireDamage);
    }

    private void Modify(Stat stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = stat.GetValue() * percentageModifier;
            
            stat.AddModifier(modifier);
        }
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
