using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; // 1 point increase damage by 1 and crit power by 1%
    public Stat agility; // 1 point increase evasion by 1% and crit chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 3 or 5 points

    [Header("Defensive stats")] 
    public Stat maxHp;
    public Stat armor;
    public Stat evasion;

    [Header("Offensive stats")] 
    [SerializeField] private float defaultCritChance = 150;
    public Stat damage;
    public Stat critChance;
    public Stat critPower;
    
    [SerializeField] private float currentHp;
    protected virtual void Start()
    {
        critChance.SetDefaultValue(defaultCritChance);
        currentHp = maxHp.GetValue();
    }

    public virtual void DoDamage(CharacterStats target)
    {
        if (CanAvoidAttack(target))
        {
            return;
        }

        float totalDamage = damage.GetValue() + strength.GetValue();
        
        totalDamage = CalculateTotalDamageWithArmor(target, totalDamage);
        // Trừ giáp trước rồi mới nhân thêm % chí mạng nếu có.
        if (CanCrit())
        {
            totalDamage = CalculateTotalDamageWithCritical(totalDamage);
        }
        target.TakeDamage(totalDamage);
        
    }

    private float CalculateTotalDamageWithArmor(CharacterStats target, float totalDamage)
    {
        totalDamage -= target.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, float.MaxValue);
        return totalDamage;
    }

    private bool CanAvoidAttack(CharacterStats target)
    {
        float totalEvasion = target.evasion.GetValue() + target.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Missed");
            return true;
        }

        return false;
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
    }
    
    protected virtual void Die()
    {
        
    }

    private bool CanCrit()
    {
        float totalCritChance = critChance.GetValue() + agility.GetValue();
        
        if (Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }

        return false;
    }

    private float CalculateTotalDamageWithCritical(float damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
        float totalDamage = damage * totalCritPower;

        return totalDamage;
    }
}
