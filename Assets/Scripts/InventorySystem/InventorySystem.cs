using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    [System.Serializable]
    public struct DefaultWeapon
    {
        [SerializeField]
        public InventoryItem weaponItem;
        [SerializeField]
        public bool isDefault;
    }

    [SerializeField]
    public List<DefaultWeapon> defaultWeapons;
    private Dictionary<InventoryItem.ItemType, List<InventoryItem>> items;

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

    public List<InventoryItem> GetAllPickedItems()
    {
        List<InventoryItem> queriedItems = new List<InventoryItem>();

        foreach (KeyValuePair<InventoryItem.ItemType, List<InventoryItem>> inventoryItems in items)
        {
            foreach (InventoryItem iteratingItem in inventoryItems.Value)
            {
                if (iteratingItem.isPicked)
                {
                    queriedItems.Add(iteratingItem);
                }
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

    public void GrabItem(InventoryItem newItem)
    {
        if (newItem.index > 0) {
            newItem.isPicked = true;
            return;
        }
        items[newItem.itemType].Add(newItem);
        newItem.index = items[newItem.itemType].Count - 1;
    }

    public void DeleteItem(InventoryItem deletedItem)
    {
        deletedItem.index = -1;
        deletedItem.isPicked = false;
        items[deletedItem.itemType].Remove(deletedItem);
    }

    public void Clear()
    {
        items.Clear();
    }
    #endregion

    #region Unity Logic
    public void Awake()
    {

        items = new Dictionary<InventoryItem.ItemType, List<InventoryItem>>();

        InventoryItem.ItemType[] itemTypes = System.Enum.GetValues(typeof(InventoryItem.ItemType)) as InventoryItem.ItemType[];
        for (int i = 0; i < itemTypes.Length; i++)
        {
            items.Add(itemTypes[i], new List<InventoryItem>());
        }

        foreach (DefaultWeapon defaultWeapon in defaultWeapons)
        {
            defaultWeapon.weaponItem.isPicked = defaultWeapon.isDefault;
            defaultWeapon.weaponItem.itemType = InventoryItem.ItemType.kItemTypeWeapon;
            defaultWeapon.weaponItem.index = items[defaultWeapon.weaponItem.itemType].Count;
            items[defaultWeapon.weaponItem.itemType].Add(defaultWeapon.weaponItem);
        }

    }
    #endregion

}
