using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{

    public enum ItemType
    {
        kItemTypeWeapon,
        kItemTypeAmmo,
        kItemTypeHP,
        kItemTypeArmour,
        kItemTypePowerUp,

    }

    internal int index = -1;
    internal bool isPicked = false;
    public ItemType itemType;

}
