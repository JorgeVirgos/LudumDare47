using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : InventoryItem {

  public enum KeyNumber {
    kKeyNumberNone = 0,
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
  public AudioClip PickUpSound;

  private AudioSource SoundSource;
  private bool bShouldDestroy;
  private float DestroyTime = 0.0f;

  static Color[] Colors =
  {
    Color.red,
    Color.green,
    Color.yellow,
    Color.blue,
    Color.magenta
  };

  static public Color GetKeyColor(KeyNumber key)
  {
    if (key == KeyNumber.kKeyNumberNone)
      Debug.LogError("GET DOWN MISTER OBAMA THIS IS A KEY WITHOUT A NUMBER");
    return Colors[(int)key - 1];
  }

  void Start()
  {
    if (itemType == ItemType.kItemTypeKey)
    {
      MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
      foreach (MeshRenderer mr in mrs)
      {
        mr.material.color = GetKeyColor(KeyTag);
      }
    }

    SoundSource = GetComponent<AudioSource>();
    if(!SoundSource) {
      SoundSource = gameObject.AddComponent<AudioSource>();
      SoundSource.playOnAwake = false;
    }
    
    bShouldDestroy = false;
  }

  // Update is called once per frame
  void Update() {
    float rotateValue = RotateSpeed * Time.deltaTime;
    transform.Rotate(0.0f, rotateValue, 0.0f);

    float BobbingValue = Mathf.Sin(Time.time) * Time.deltaTime * 0.1f;
    transform.Translate(0.0f, BobbingValue, 0.0f);

    if(bShouldDestroy) {
      DestroyTime += Time.deltaTime;
      if(DestroyTime >= 3.0f) {
        Destroy(gameObject);
      }
    }
  }

  void OnTriggerEnter(Collider collider) {
    if(collider.gameObject.tag == "Player") {
      InventorySystem Inventory = (collider.gameObject.GetComponent<PlayerController>()).Inventory;
      if(Inventory) {
        SoundSource.PlayOneShot(PickUpSound, 0.5f);
        switch (itemType) {
          case InventoryItem.ItemType.kItemTypeWeapon:
            {
              Inventory.GrabItem(this);
              bShouldDestroy = true;
              gameObject.GetComponent<BoxCollider>().enabled = false;
              gameObject.transform.GetChild(0).gameObject.SetActive(false);
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
              gameObject.GetComponent<BoxCollider>().enabled = false;
              gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }break;
          case InventoryItem.ItemType.kItemTypeHP:
            {
              HealthComponent health = collider.gameObject.GetComponent<HealthComponent>();
              if(health) health.Heal(HealAmount);
              gameObject.GetComponent<BoxCollider>().enabled = false;
              gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }break;
          case InventoryItem.ItemType.kItemTypeArmour:
            {
              HealthComponent health = collider.gameObject.GetComponent<HealthComponent>();
              if(health) health.RestoreArmor(ArmorAmount);
              gameObject.GetComponent<BoxCollider>().enabled = false;
              gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }break;
          case InventoryItem.ItemType.kItemTypePowerUp:
            {
              // Get Power Up
            }break;
          case InventoryItem.ItemType.kItemTypeKey:
            {
              Inventory.GrabItem(this);
              gameObject.GetComponent<BoxCollider>().enabled = false;
              gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }break;
        }
      }
    }
  }
}
