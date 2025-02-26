using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX entityFX;
    [Header("Major stats")]
    public Stat strength; // 1 point increase damage by 1 and crit power by 1%
    public Stat agility; // 1 point increase evasion by 1% and crit chance by 1%
    public Stat intelligence; // 1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increase health by 3 or 5 points
    [SerializeField] public float healthIncreasePerVitality = 5;

    [Header("Defensive stats")] 
    public Stat maxHp;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    [SerializeField] private float magicResistanceIncreasePerIntelligence = 3;
    [SerializeField] private float slowPercentage = 0.2f;
    [SerializeField] private float shockedDetectedRadius = 25f;
    [SerializeField] private GameObject shockStrikePrefab;
    [SerializeField] private float shockDamage;
    
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
    
    public float currentHp;
    public System.Action onHealthChanged;
    protected bool isDead;
    protected virtual void Start()
    {
        critChance.SetDefaultValue(defaultCritChance);
        currentHp = GetMaxHp();
        
        entityFX = GetComponent<EntityFX>();
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

        if (isIgnited)
        {
            ApplyIgniteDamage();
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
    
    protected virtual void Die()
    {
        isDead = true;
    }

    #region Magical Damage and Ailments
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
        
        AttemptToApplyAilment(target, _fireDamage, _iceDamage, _lightningDamage);
    }

    private void AttemptToApplyAilment(CharacterStats target, float _fireDamage, float _iceDamage,
        float _lightningDamage)
    {
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

        if (canApplyShock)
        {
            target.SetupShockStrikeDamage(_lightningDamage * .1f);
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
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if (ignite && canApplyIgnite)
        {
            isIgnited = ignite;
            ignitedTimer = spellDuration;
            entityFX.IgniteFxFor(spellDuration); 
        }
        
        if (chill && canApplyChill)
        {
            isChilled = chill;
            chilledTimer = spellDuration;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, spellDuration);
            entityFX.ChillFxFor(spellDuration);
        }
        
        if (shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearestTargetWithShockStrike();
            }
        }
        
    }
    
    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthBy(ignitedDamage);
            if (currentHp < 0 && !isDead)
            {
                Die();
            }
            ignitedDamageTimer = ignitedDamageCooldown;
            // Burn damage over time (0.3s)
        }
    }

    public void ApplyShock(bool shock)
    {
        if (isShocked)
        {
            return;
        }
        
        isShocked = shock;
        shockedTimer = spellDuration;
        entityFX.ShockFxFor(spellDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, shockedDetectedRadius);
        
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Enemy enemy) && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().SetupThunder(shockDamage
                , closestEnemy.GetComponent<CharacterStats>()
            );
        }
    }

    public void SetupIgniteDamage(float damage)
    {
        ignitedDamage = damage;
    }

    public void SetupShockStrikeDamage(float damage)
    {
        shockDamage = damage;
    }
    
    #endregion

    #region Calculate character stats
    
    public virtual void TakeDamage(float damage)
    {
        DecreaseHealthBy(damage);
        
        GetComponent<Entity>().DamageImpact();
        entityFX.StartCoroutine("FlashFX");
        
        if (currentHp <= 0)
        {
            Die();
        }

        onHealthChanged();
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
    
    private bool CanCrit()
    {
        float totalCritChance = critChance.GetValue() + agility.GetValue();
        
        if (Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }

        return false;
    }
    
    protected virtual void DecreaseHealthBy(float damage)
    {
        currentHp -= damage;
        
        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
        // giảm máu chir là giảm máu đơn thuaanf thôi chứ ko apply effect.
    }

    private float CalculateTotalDamageWithCritical(float damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f; 
        float totalDamage = damage * totalCritPower;

        return totalDamage;
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
    
    public float GetMaxHp()
    {
        return maxHp.GetValue() 
               + vitality.GetValue() * healthIncreasePerVitality;
    }

    #endregion
}
