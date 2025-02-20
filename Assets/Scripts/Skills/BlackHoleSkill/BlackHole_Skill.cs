using UnityEngine;

public class BlackHole_Skill : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackHole = Instantiate(blackHolePrefab);
        BlackHole_Skill_Controller blackHoleSkillController = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        blackHoleSkillController.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown);
    }
}
