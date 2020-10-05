using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    private struct CanvasSelectable
    {
        public Image Background;
        public Image Selectable;
    }

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

    [System.Serializable]
    public struct InventoryItemTypeCanvas
    {
        [SerializeField]
        public GameObject canvasPrefab;

        [SerializeField]
        public Color notPickedColor;

        [SerializeField]
        public Color pickedColor;

        [SerializeField]
        public Color highlightColor;
    }

    [SerializeField]
    public GameObject canvas;

    [SerializeField]
    public InventoryItemTypeCanvas weaponItemTypeCanvas;


    private List<CanvasSelectable> weaponImages;
    private int actualWeaponImageIndex = 0;

    [SerializeField]
    public List<DefaultWeapon> defaultWeapons;
    private Dictionary<InventoryItem.ItemType, List<InventoryItem>> items;

    #region Inventory System Utility
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

    public void ReloadInventory(GameObject player)
    {


        InventoryItem pistol = player.transform.GetChild(0).GetChild(0).GetComponent<InventoryItem>();
        InventoryItem smg = player.transform.GetChild(0).GetChild(1).GetComponent<InventoryItem>();
        InventoryItem shotgun = player.transform.GetChild(0).GetChild(2).GetComponent<InventoryItem>();

        player.GetComponent<PlayerController>().CurrentWeapon = pistol as Weapon;

        DefaultWeapon dw = defaultWeapons[0];
        dw.weaponItem = pistol;
        defaultWeapons[0] = dw;

        dw = defaultWeapons[1];
        dw.weaponItem = smg;
        defaultWeapons[1] = dw;

        dw = defaultWeapons[2];
        dw.weaponItem = shotgun;
        defaultWeapons[2] = dw;

        canvas = player.transform.GetChild(0).GetChild(3).gameObject;

        createWeaponCanvas(false);

    }

    public void createWeaponCanvas(bool firstTime = true)
    {
        GameObject weaponsCanvas = canvas.transform.Find("Weapons").gameObject;
        weaponImages = new List<CanvasSelectable>();

        if (firstTime)
        {
            foreach (DefaultWeapon defaultWeapon in defaultWeapons)
            {
                defaultWeapon.weaponItem.isPicked = defaultWeapon.isDefault;
                defaultWeapon.weaponItem.itemType = InventoryItem.ItemType.kItemTypeWeapon;
                defaultWeapon.weaponItem.index = items[defaultWeapon.weaponItem.itemType].Count;
                items[defaultWeapon.weaponItem.itemType].Add(defaultWeapon.weaponItem);
                GameObject newWeaponCanvas = Instantiate(weaponItemTypeCanvas.canvasPrefab);
                newWeaponCanvas.transform.SetParent(weaponsCanvas.transform);
                if (defaultWeapon.weaponSprite != null)
                {
                    CanvasSelectable newSelectable = new CanvasSelectable();
                    newSelectable.Background = newWeaponCanvas.transform.GetChild(0).GetComponent<Image>();
                    newSelectable.Selectable = newWeaponCanvas.transform.GetChild(1).GetComponent<Image>();
                    newSelectable.Selectable.sprite = defaultWeapon.weaponSprite;
                    newSelectable.Background.enabled = false;
                    if (defaultWeapon.weaponItem.isPicked)
                    {
                        newSelectable.Selectable.color = weaponItemTypeCanvas.pickedColor;
                    }
                    else
                    {
                        newSelectable.Selectable.color = weaponItemTypeCanvas.notPickedColor;
                    }
                    weaponImages.Add(newSelectable);
                }
            }
            actualWeaponImageIndex = 0;
            weaponImages[actualWeaponImageIndex].Background.enabled = true;
            weaponImages[actualWeaponImageIndex].Background.color = weaponItemTypeCanvas.highlightColor;

        }
        else
        {
            int index = 0;
            foreach (DefaultWeapon defaultWeapon in defaultWeapons)
            {
                defaultWeapon.weaponItem.isPicked = defaultWeapon.isDefault;
                defaultWeapon.weaponItem.itemType = InventoryItem.ItemType.kItemTypeWeapon;
                defaultWeapon.weaponItem.index = index;
                items[defaultWeapon.weaponItem.itemType][index] = defaultWeapon.weaponItem;

                GameObject newWeaponCanvas = Instantiate(weaponItemTypeCanvas.canvasPrefab);
                newWeaponCanvas.transform.SetParent(weaponsCanvas.transform);
                if (defaultWeapon.weaponSprite != null)
                {
                    CanvasSelectable newSelectable = new CanvasSelectable();
                    newSelectable.Background = newWeaponCanvas.transform.GetChild(0).GetComponent<Image>();
                    newSelectable.Selectable = newWeaponCanvas.transform.GetChild(1).GetComponent<Image>();
                    newSelectable.Selectable.sprite = defaultWeapon.weaponSprite;
                    newSelectable.Background.enabled = false;
                    if (defaultWeapon.weaponItem.isPicked)
                    {
                        newSelectable.Selectable.color = weaponItemTypeCanvas.pickedColor;
                    }
                    else
                    {
                        newSelectable.Selectable.color = weaponItemTypeCanvas.notPickedColor;
                    }
                    weaponImages.Add(newSelectable);
                }

                ++index;
            }
            actualWeaponImageIndex = 0;
            weaponImages[actualWeaponImageIndex].Background.enabled = true;
            weaponImages[actualWeaponImageIndex].Background.color = weaponItemTypeCanvas.highlightColor;

        }

    }


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
            Weapon w = newItem.gameObject.GetComponent<Weapon>();
            if (w)
            {
                items[newItem.itemType][(int)w.weaponType].isPicked = true;
                weaponImages[(int)w.weaponType].Selectable.color = weaponItemTypeCanvas.pickedColor;
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

    public void SelectWeapon(int weaponIndex)
    {
        weaponImages[actualWeaponImageIndex].Background.enabled = false;
        //         weaponImages[actualWeaponImageIndex].color = weaponItemTypeCanvas.pickedColor;
        weaponImages[weaponIndex].Background.color =
            weaponItemTypeCanvas.highlightColor;
        weaponImages[weaponIndex].Background.enabled = true;
        actualWeaponImageIndex = weaponIndex;
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

        createWeaponCanvas(true);

    }
    #endregion

}
