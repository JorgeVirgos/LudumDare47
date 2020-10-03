using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InventoryItem {

  public GameObject Projectile;
  public float ShotCooldown = 0.0f;
  public bool bIsShooting = false;
  public int MaxAmmo;
  public int MaxClipAmmo;

  protected int CurrentAmmo;
  protected int CurrentClipAmmo;
  protected float CurrentShotCooldown = 0.0f;
  protected float AmmunitionQuantity;

  // Start is called before the first frame update
  void Start() {
    CurrentShotCooldown = ShotCooldown;
    bIsShooting = false;
    CurrentAmmo = MaxAmmo;
    CurrentClipAmmo = MaxClipAmmo;
  }

  // Update is called once per frame
  void Update() {
  }

  public virtual void Shoot() {
    CurrentClipAmmo -= 1;
  }

  public void Reload() {
    if(CurrentAmmo >= MaxClipAmmo) {
      CurrentClipAmmo = MaxClipAmmo;
      CurrentAmmo -= MaxClipAmmo;
    } else {
      CurrentClipAmmo = CurrentAmmo;
      CurrentAmmo = 0;
    }
  }
}
