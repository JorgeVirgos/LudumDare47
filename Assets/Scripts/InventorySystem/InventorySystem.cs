﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    private Dictionary<InventoryItem.ItemType, List<InventoryItem>> items;

    internal int actualWepaonIndex = 0;

    #region Inventory System Utility
    public List<InventoryItem> GetAllInventoryItems()
    {

        List<InventoryItem> queriedItems = new List<InventoryItem>();

        foreach (KeyValuePair<InventoryItem.ItemType, List<InventoryItem>> inventoryItems in items)
        {
            foreach (InventoryItem iteratingItem in inventoryItems.Value)
            {
                queriedItems.Add(iteratingItem);
            }
        }

        return queriedItems;
    }

    public List<InventoryItem> GetInventoryItemsByType(InventoryItem.ItemType queriedType)
    {
        return items[queriedType];
    }

    public InventoryItem GetInventoryItemByIndex(InventoryItem.ItemType itemType, int itemIndex)
    {
        if (!items.ContainsKey(itemType)) { return null; }
        if (itemIndex >= items[itemType].Count) { return null; }

        return items[itemType][itemIndex];
    }

    public void AddItem(InventoryItem newItem)
    {
        if (newItem.index > 0) { return; }
        newItem.isPicked = true;
        items[newItem.type].Add(newItem);
        newItem.index = items[newItem.type].Count - 1;
    }

    public void DeleteItem(InventoryItem deletedItem)
    {
        deletedItem.index = -1;
        deletedItem.isPicked = false;
        items[deletedItem.type].Remove(deletedItem);
    }

    public void Clear()
    {
        items.Clear();
    }

    public void SetWeapon(int newWeaponIndex)
    {

        if (items[InventoryItem.ItemType.kItemTypeWeapon].Count >= newWeaponIndex)
        {
            return;
        }

        actualWepaonIndex = newWeaponIndex;

    }

    #endregion

    #region Unity Logic
    public void Awake()
    {
        items = new Dictionary<InventoryItem.ItemType, List<InventoryItem>>();
    }
    #endregion

}
