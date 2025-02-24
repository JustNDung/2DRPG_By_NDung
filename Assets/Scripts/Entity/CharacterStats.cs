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
    public Stat magicResistance;

    [Header("Magic stats")]
    [SerializeField] private float magicResistanceIncreasePerIntelligence = 3;
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
    

    public bool isIgnited; // does damage over time
    public bool isChilled; // reduce armor by 20%
    public bool isShocked; // reduce accuracy by 20%
    [SerializeField] private float spellDuration =  2f;

    private float ignitedTimer;
    [SerializeField] private float ignitedDamageCooldown = 0.3f;
    private float ignitedDamageTimer;
    [SerializeField] private float ignitedDamage;

    private float chilledTimer;
    private float shockedTimer;
    
    [Header("Offensive stats")] 
    [SerializeField] private float defaultCritChance = 150;
    public Stat damage;
    public Stat critChance;
    public Stat critPower;
    
    [SerializeField] private float evasionBoostWhenShocked = 20;
    [SerializeField] private float armorReductionWhenChilled = 0.8f;
    
    [SerializeField] private float currentHp;
    protected virtual void Start()
    {
        critChance.SetDefaultValue(defaultCritChance);
        currentHp = maxHp.GetValue();
    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }
        
        if (chilledTimer < 0)
        {
            isChilled = false;
        }
        
        if (shockedTimer < 0)
        {
            isShocked = false;
        }
        
        if (ignitedDamageTimer < 0 && isIgnited)
        {
            Debug.Log("Take burn damage " + ignitedDamage);
            currentHp -= ignitedDamage;
            if (currentHp < 0)
            {
                Die();
            }
            ignitedDamageTimer = ignitedDamageCooldown;
            // Burn damage over time (0.3s)
        }
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
        //target.TakeDamage(totalDamage);
        DoMagicalDamage(target);
    }
    
    public virtual void DoMagicalDamage(CharacterStats target)
    {
        float _fireDamage = fireDamage.GetValue();
        float _iceDamage = iceDamage.GetValue();
        float _lightningDamage = lightningDamage.GetValue();
        float totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicDamage = CalculateTotalMagicDamage(target, totalMagicDamage);
        
        target.TakeDamage(totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        } 
        
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;
        
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                target.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Ignited");
                return;
            }
            if (Random.value < .5f && _iceDamage > 0)
            {
                canApplyChill = true;
                target.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Chilled");
                return;
            }
            if (Random.value < .5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                target.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Shocked");
                return;
            }
        }

        if (canApplyIgnite)
        {
            target.SetupIgniteDamage(_fireDamage * .2f);
            // Apply 20% of fire damage per 0.3 second.
        }
        
        target.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private float CalculateTotalMagicDamage(CharacterStats target, float totalMagicDamage)
    {
        totalMagicDamage -= target.magicResistance.GetValue() 
                            + (target.intelligence.GetValue() * magicResistanceIncreasePerIntelligence);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, float.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilments(bool ignite, bool chill, bool shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (ignite)
        {
            isIgnited = ignite;
            ignitedTimer = spellDuration;
        }
        
        if (chill)
        {
            isChilled = chill;
            chilledTimer = spellDuration;
        }
        
        if (shock)
        {
            isShocked = shock;
            shockedTimer = spellDuration;
        }
        
    }
    
    public void SetupIgniteDamage(float damage)
    {
        ignitedDamage = damage;
    }

    private float CalculateTotalDamageWithArmor(CharacterStats target, float totalDamage)
    {
        if (target.isChilled)
        {
            totalDamage -= target.armor.GetValue() * armorReductionWhenChilled;
        }
        else
        {
            totalDamage -= target.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, float.MaxValue);
        return totalDamage;
    }
    
    private bool CanAvoidAttack(CharacterStats target)
    {
        float totalEvasion = target.evasion.GetValue() + target.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += evasionBoostWhenShocked;
        }
        
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Missed");
            return true;
        }

        return false;
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
