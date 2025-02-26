
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
    }

    public void AddItem(ItemData itemData)
    {
        if (inventoryDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(itemData, newItem);
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        if (inventoryDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(itemData);
            }
            else
            {
                value.RemoveStack(); 
            }
        }
    }
}
