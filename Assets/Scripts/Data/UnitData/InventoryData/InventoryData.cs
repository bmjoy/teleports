﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

[System.Serializable]
public class InventoryData {
    
    [SerializeField] private int maxSlots = 32;
    [SerializeField] private EquipmentData equipmentData;
    [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 8)] private List<InventorySlotData> invSlots;

    public InventoryData()
    {
        Initialize();
    }

    public void Initialize()
    {
        maxSlots = 32;
        equipmentData = new EquipmentData();
        Utils.InitWithNew(ref invSlots, maxSlots);
    }

    public void Add(ItemData item)
    {
        //stack on existing slot
        foreach(InventorySlotData slot in invSlots)
        {
            if(!slot.Empty && slot.Item == item)
            {
                slot.Add(item);
                return;
            }
        }

        //add to empty slot
        foreach(InventorySlotData slot in invSlots)
        {
            if (slot.Empty)
            {
                slot.Add(item);
                return;
            }
        }

        //add extra slot if possible
        if(invSlots.Count < maxSlots)
        {
            InventorySlotData slot = new InventorySlotData();
            slot.Add(item);
            invSlots.Add(slot);
        }

        Debug.Log("Inventory is full!");
        return;
    }

    public bool Contains(ItemData item)
    {
        foreach(InventorySlotData slot in invSlots)
        {
            if (slot.Item == item) return true;
        }
        return false;
    }

    public void Remove(ItemData item)
    {
        foreach (InventorySlotData slot in invSlots)
        {
            if (!slot.Empty && slot.Item == item)
            {
                slot.Pop();
                return;
            }
        }
    }

    public void Equip(string itemName)
    {
        Equip(GetItemByName(itemName));
    }

    public void Equip(ItemData item)
    {
        if (Contains(item))
        {
            Equip(InvSlotIdOf(item));
        }
    }

    public void Equip(int inventorySlotId)
    {
        if (IsValidSlotId(inventorySlotId))
        {
            InventorySlotData inventorySlot = invSlots[inventorySlotId];
            if (inventorySlot.Empty)
                return;

            ItemData item = inventorySlot.Item;
            if(equipmentData.CanEquip(item).Status == EquipmentData.CanEquipStatus.Yes)
            {
                equipmentData.Equip(item);
                Remove(item);
            }
        }
    }

    /*public void Unequip(EquipmentSlotType slot)
    {
        ItemData unequippedItem = equipmentData.Unequip(slot);
        Add(unequippedItem);
    }*/

    public InventorySlotData GetInventorySlotData(int inventorySlotId)
    {
        if (IsValidSlotId(inventorySlotId))
            return invSlots[inventorySlotId];
        else
            return null;
    }

    public IList<EquipmentData.EquippedItemInfo> GetEquippedItems()
    {
        return equipmentData.GetEquippedItems();
    }

    public List<ItemData> GetAllItemsInInventory()
    {
        List<ItemData> result = new List<ItemData>();
        foreach(var slot in invSlots)
        {
            if(!slot.Empty)
            {
                result.Add(slot.Item);
            }
        }
        return result;
    }

    public IList<ItemData> GetAllItems()
    {
        var result = GetAllItemsInInventory();
        var result2 = new List<ItemData>();
        foreach(var slotInfo in GetEquippedItems())
        {
            result2.Add(slotInfo.Item);
        }
        result.AddRange(result2);
        return result.AsReadOnly();
    }

    public void CorrectInvalidData()
    {
        if (maxSlots == 0)
        {
            Initialize();
        }
    }

    List<ItemData> GetItems(IEnumerable<ItemID> itemIds)
    {
        List<ItemData> result = new List<ItemData>();

        foreach (ItemID id in itemIds)
        {
            ItemData itemData = MainData.CurrentGameData.GetItem(id);
            if (itemData != null)
            {
                result.Add(itemData);
            }

        }
        return result;
    }

    ItemData GetItemByName(string name)
    {
        foreach(var invSlot in invSlots)
        {
            if(!invSlot.Empty && invSlot.Item.UniqueName == name)
            {
                return invSlot.Item;
            }
        }
        return null;
    }

    int InvSlotIdOf(ItemData item)
    {
        Debug.Assert(Contains(item));
        for(int i = 0; i<invSlots.Count; i++)
        {
            if(invSlots[i].Item == item)
            {
                return i;
            }
        }
        return -1;
    }

    public EquipmentData EquipmentData
    {
        get { return equipmentData; }
    }

    private bool IsValidSlotId(int id)
    {
        return id >= 0 && id < invSlots.Count;
    }
   
}
