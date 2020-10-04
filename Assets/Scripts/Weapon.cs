using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : InventoryItem {

  internal enum WeaponType {
    kWeaponTypePistol = 0,
    kWeaponTypeMachineGun = 1,
    kWeaponTypeShotgun = 2,
  }

  public GameObject Projectile;
  public float ShotCooldown = 0.0f;
  public float WeaponDamage = 10.0f;
  public bool bIsShooting = false;
  public int MaxAmmo;
  public int MaxClipAmmo;
  public AudioClip ShootSound;
  public AudioClip ReloadSound;

  internal WeaponType weaponType;

  protected int CurrentAmmo;
  protected int CurrentClipAmmo;
  protected float CurrentShotCooldown = 0.0f;
  protected float AmmunitionQuantity;

  private AudioSource AudioComp;

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
    AudioComp.PlayOneShot(ShootSound);
  }

  public void Reload() {
    AudioComp.PlayOneShot(ReloadSound);
    if(CurrentAmmo >= MaxClipAmmo) {
      CurrentClipAmmo = MaxClipAmmo;
      CurrentAmmo -= MaxClipAmmo;
    } else {
      CurrentClipAmmo = CurrentAmmo;
      CurrentAmmo = 0;
    }
  }

  public void RecoverAmmo(int ammo) {
    CurrentAmmo += ammo;
    if(CurrentAmmo > MaxAmmo) {
      CurrentAmmo = MaxAmmo;
    }
  }
}
