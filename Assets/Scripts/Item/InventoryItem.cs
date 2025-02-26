using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData itemData)
    {
        data = itemData;
        AddStack();
    }
    
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
    
}
