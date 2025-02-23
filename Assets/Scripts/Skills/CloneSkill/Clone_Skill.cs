using System;
using System.Collections;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private int clonePoolSize;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    [SerializeField] private float attackCheckRadius = 25f;
    [SerializeField] private float cloneDelay = 0.4f;
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;
    [Header("Clone duplication")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float createCloneRate = 35f;
    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;
    
    private GameObjectPooling clonePool;

    protected override void Start()
    {
        base.Start(); 
        clonePool = new GameObjectPooling(clonePrefab, clonePoolSize);
    }

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }
        GameObject clone = clonePool.Get();
        clone.GetComponent<Clone_Skill_Controller>().SetupClone(clonePosition, cloneDuration
            , canAttack, offset
            , FindClosestEnemy(clone.transform, attackCheckRadius), canDuplicateClone
            , createCloneRate
        );
    }
    
    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    
    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }
    
    public void CreateCloneOnCounterAttack(Transform clonePosition)
    {
        if (createCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(clonePosition, new Vector3(2 * player.facingDirection, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelay(Transform clonePosition, Vector3 offset)
    {
        yield return new WaitForSeconds(cloneDelay);
        CreateClone(clonePosition, offset);
    }
}
