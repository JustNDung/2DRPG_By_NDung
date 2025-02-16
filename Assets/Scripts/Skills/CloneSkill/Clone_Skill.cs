using System;
using UnityEngine;

public class Clone_Skill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private int clonePoolSize;
    [SerializeField] private float cloneDuration;
    private GameObjectPooling clonePool;

    private void Start()
    {
        clonePool = new GameObjectPooling(clonePrefab, clonePoolSize);
    }

    public void CreateClone(Transform clonePosition)
    {
        GameObject clone = clonePool.Get();
        
        clone.GetComponent<Clone_Skill_Controller>().SetupClone(clonePosition, cloneDuration);
        
    }
}
