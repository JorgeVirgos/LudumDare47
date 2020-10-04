using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

  [System.Serializable]
  public struct DefaultWeapon
  {
    [SerializeField]
    public InventoryItem weaponItem;
    [SerializeField]
    public bool isDefault;
    [SerializeField]
    public Sprite weaponSprite;
  }

  [SerializeField]
  public GameObject canvas;

  [SerializeField]
  public GameObject weaponCanvasPrefab;

  [SerializeField]
  public List<DefaultWeapon> defaultWeapons;
  private Dictionary<InventoryItem.ItemType, List<InventoryItem>> items;

  public bool HasThisKey(PickableObject.KeyNumber key)
  {
    List<InventoryItem> keys = items[InventoryItem.ItemType.kItemTypeKey];
    foreach (InventoryItem item in keys)
    {
      PickableObject po = (PickableObject)item;
      if (po != null && po.KeyTag == key)
      {
        return true;
      }
      
    }
    return false;
  }


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
    if (newItem.itemType == InventoryItem.ItemType.kItemTypeWeapon)
    {
      Weapon w = newItem as Weapon;
      if (w)
      {
        items[newItem.itemType][(int)w.weaponType].isPicked = true;
        return;
      }
    }
    if (newItem.index >= 0)
    {
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

    if (canvas == null)
    {
      Debug.LogError("Error: Canvas Object not set");
      return;
    }

    GameObject weaponsCanvas = canvas.transform.Find("Weapons").gameObject;

    foreach (DefaultWeapon defaultWeapon in defaultWeapons)
    {
      defaultWeapon.weaponItem.isPicked = defaultWeapon.isDefault;
      defaultWeapon.weaponItem.itemType = InventoryItem.ItemType.kItemTypeWeapon;
      defaultWeapon.weaponItem.index = items[defaultWeapon.weaponItem.itemType].Count;
      items[defaultWeapon.weaponItem.itemType].Add(defaultWeapon.weaponItem);
      GameObject newWeaponCanvas = Instantiate(weaponCanvasPrefab);
      newWeaponCanvas.transform.SetParent(weaponsCanvas.transform);
      if (defaultWeapon.weaponSprite != null)
        newWeaponCanvas.GetComponent<Image>().sprite = defaultWeapon.weaponSprite;
    }

  }
  #endregion

}
