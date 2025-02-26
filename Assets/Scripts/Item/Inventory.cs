
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    [Header("Inventory UI")] [SerializeField]
    private Transform inventorySlotParent;

    private UI_ItemSlot[] itemSlots;
    
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
        
        itemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        // Find all the component and add all of it into array. 
    }

    private void UpdateUISlot()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            itemSlots[i].UpdateSlot(inventoryItems[i]);
        }
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
        
        UpdateUISlot();
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
        
        UpdateUISlot();
    }
}
