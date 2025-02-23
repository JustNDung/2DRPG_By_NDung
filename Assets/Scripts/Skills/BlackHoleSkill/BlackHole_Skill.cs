using UnityEngine;

public class BlackHole_Skill : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackHoleDuration;
    
    private BlackHole_Skill_Controller blackHoleSkillController;
    
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
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        blackHoleSkillController = newBlackHole.GetComponent<BlackHole_Skill_Controller>();
        blackHoleSkillController.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackHoleDuration);
    }

    public bool BlackHoleFinshed()
    {
        if (!blackHoleSkillController)
        {
            return false;
        }
        
        if (blackHoleSkillController.playerCanExitState)
        {
            blackHoleSkillController = null;
            return true;
        }

        return false;
    }

    public float GetBlackHoleRadius()
    {
        return maxSize / 2;
    }
}
