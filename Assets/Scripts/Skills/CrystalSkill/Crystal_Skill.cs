using UnityEngine;
using System.Collections.Generic;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;
    
    [Header("Explosive crystal")] 
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")] 
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float attackCheckRadius = 25f;

    [Header("Multi stacking crystal")] [SerializeField]
    private int amountOfStacks;
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private float multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();
    
    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiStacks())
        {
            return;
        }
        
        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy)
            {
                return;
            }
            // Dịch chuyển player :
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller crystalSkillController = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        crystalSkillController.SetupCrystal(crystalDuration, canExplode 
            , canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform, attackCheckRadius)
            , player
        ); 
    }
    
    public void CurrentCrystalChooseRandomTarget()
    {
        if (currentCrystal != null)
        {
            currentCrystal.GetComponent<Crystal_Skill_Controller>()?.ChooseRandomEnemy();
        }
    }

    private bool CanUseMultiStacks()
    {
        if (canUseMultiStacks)
        {
            if (crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                {
                    Invoke("ResetAbility", useTimeWindow);
                }
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
                
                crystalLeft.Remove(crystalToSpawn);
                newCrystal.GetComponent<Crystal_Skill_Controller>().SetupCrystal(crystalDuration, canExplode 
                    , canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform, attackCheckRadius)
                    , player
                );
            }
            else
            {
                cooldown = multiStackCooldown;
                RefillCrystal();
            }
            return true;
        }

        return false;
    }
    
    private void RefillCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;
        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
        {
            return;
        }
        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }
}
