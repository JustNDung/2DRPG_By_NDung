
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")] 
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots;
    private UI_EquipmentSlot[] equipmentSlots;
    
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
        equipment = new List<InventoryItem>();
        inventory = new List<InventoryItem>();
        stash = new List<InventoryItem>();

        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();
        
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        // Find all the component and add all of it into array. 
    }

    public void EquipItem(ItemData item)
    {
        // Can not equip an equipment which is the same type as an equipment player has equipped.
        ItemData_Equipment newEquipment = item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> _equipment in equipmentDictionary)
        {
            if (_equipment.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = _equipment.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
            // Unequip item which exists with the same item type
        }
        
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        RemoveItem(item);
        UpdateUISlot();
    }

    private void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
        }
    }

    // TODO
    // Optimizing Update UiSlot
    private void UpdateUISlot()
    {
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
         
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }
        
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }
        
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }

        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> _equipment in equipmentDictionary)
            {
                if (_equipment.Key.equipmentType == equipmentSlots[i].equipmentType)
                {
                    equipmentSlots[i].UpdateSlot(_equipment.Value);
                }
            }
        }
    }

    public void AddItem(ItemData itemData)
    {
        switch (itemData.itemType)
        {
            case ItemType.Equipment:
                AddToInventory(itemData);
                break;
            case ItemType.Material:
                AddToStash(itemData);
                break;
        }
        UpdateUISlot();
    }

    // TODO 
    // Optimizing these 3 function to shorter function.
    private void AddToInventory(ItemData itemData)
    {
        if (inventoryDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            inventoryDictionary.Add(itemData, newItem);
        }
        // Update UI slot from here
    }
    
    private void AddToStash(ItemData itemData)
    {
        if (stashDictionary.TryGetValue(itemData, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            stash.Add(newItem);
            stashDictionary.Add(itemData, newItem);
        }
        // Update UI slot from here
    }

    public void RemoveItem(ItemData itemData)
    {
        if (inventoryDictionary.TryGetValue(itemData, out InventoryItem inventoryValue))
        {
            if (inventoryValue.stackSize <= 1)
            {
                inventory.Remove(inventoryValue);
                inventoryDictionary.Remove(itemData);
            }
            else
            {
                inventoryValue.RemoveStack(); 
            }
        }
        
        if (stashDictionary.TryGetValue(itemData, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(itemData);
            }
            else
            {
                stashValue.RemoveStack(); 
            }
        }
        
        UpdateUISlot();
    }
}
