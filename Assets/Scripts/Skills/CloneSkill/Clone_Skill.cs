using System;
using UnityEngine;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private int clonePoolSize;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;
    private GameObjectPooling clonePool;

    protected override void Start()
    {
        base.Start(); 
        clonePool = new GameObjectPooling(clonePrefab, clonePoolSize);
    }

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        GameObject clone = clonePool.Get();
        clone.GetComponent<Clone_Skill_Controller>().SetupClone(clonePosition, cloneDuration, canAttack, offset);
    }
}
