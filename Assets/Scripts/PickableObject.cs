using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : InventoryItem {

  public enum KeyNumber {
    kKeyNumberFirst, 
    kKeyNumberSecond, 
    kKeyNumberThird, 
    kKeyNumberFourth, 
    kKeyNumberFifth, 
  }

  public KeyNumber KeyTag;
  public float RotateSpeed = 50.0f;

  // Update is called once per frame
  void Update() {
    float rotateValue = RotateSpeed * Time.deltaTime;
    transform.Rotate(0.0f, rotateValue, 0.0f);
  }

  void OnTriggerEnter(Collider collider) {
    if(collider.gameObject.tag == "Player") {
      InventorySystem Inventory = collider.gameObject.GetComponent<InventorySystem>();
      if(Inventory) {
        switch (itemType) {
          case InventoryItem.ItemType.kItemTypeWeapon:
            {
              // Add Weapon
            }break;
          case InventoryItem.ItemType.kItemTypeAmmo:
            {
              // Reload Ammo
            }break;
          case InventoryItem.ItemType.kItemTypeHP:
            {
              // Heal Player
            }break;
          case InventoryItem.ItemType.kItemTypeArmour:
            {
              // Heal Armor
            }break;
          case InventoryItem.ItemType.kItemTypePowerUp:
            {
              // Get Power Up
            }break;
          case InventoryItem.ItemType.kItemTypeKey:
            {
              Inventory.GrabItem(this);
              gameObject.SetActive(false);
            }break;
        }
      }
    }
  }
}
