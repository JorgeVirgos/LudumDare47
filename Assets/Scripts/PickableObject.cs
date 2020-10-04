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
  public float HealAmount = 30.0f;
  public float ArmorAmount = 30.0f;

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
              Inventory.GrabItem(this);
              gameObject.SetActive(false);
            }break;
          case InventoryItem.ItemType.kItemTypeAmmo:
            {
              for(int i = 0; i < 3; ++i) {
                Weapon w = 
                  (Weapon)Inventory.GetInventoryItemByIndex(InventoryItem.ItemType.kItemTypeWeapon, i);
                if(w) {
                  w.RecoverAmmo(w.MaxAmmo / 3);
                }
              }
            }break;
          case InventoryItem.ItemType.kItemTypeHP:
            {
              HealthComponent health = collider.gameObject.GetComponent<HealthComponent>();
              if(health) health.Heal(HealAmount);
            }break;
          case InventoryItem.ItemType.kItemTypeArmour:
            {
              HealthComponent health = collider.gameObject.GetComponent<HealthComponent>();
              if(health) health.RestoreArmor(ArmorAmount);
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
