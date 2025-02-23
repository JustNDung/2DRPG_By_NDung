using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    public List<float> modifiers;
    
    public float GetBaseValue()
    {
        float finalValue = baseValue;
        
        foreach (var modifier in modifiers)
        {
            finalValue += modifier;
        }
        
        return finalValue;
    }
    
    public void AddModifier(float modifier)
    { 
        modifiers.Add(modifier);
    }
    
    public void RemoveModifier(float modifier)
    {
        modifiers.Remove(modifier);
    }
}
