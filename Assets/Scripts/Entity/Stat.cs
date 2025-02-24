using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    public List<float> modifiers;
    
    public float GetValue()
    {
        float finalValue = baseValue;
        
        foreach (var modifier in modifiers)
        {
            finalValue += modifier;
        }
        
        return finalValue;
    }
    
    public void SetDefaultValue(float value)
    {
        baseValue = value;
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
