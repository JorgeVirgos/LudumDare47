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
  public AudioClip EmptyClipSound;
  public Vector3 ReloadOffset;
  public float ReloadTime;
  public AnimationCurve ReloadCurve;

  internal WeaponType weaponType;

  protected int CurrentAmmo;
  protected int CurrentClipAmmo;
  protected float CurrentShotCooldown = 0.0f;
  protected float AmmunitionQuantity;

  private AudioSource AudioComp;
  private bool bIsReloading;
  private Vector3 LocalPosition;
  private float CurrentReloadTime;

  void Awake() {
    CurrentShotCooldown = ShotCooldown;
    bIsShooting = false;
    CurrentAmmo = MaxAmmo;
    CurrentClipAmmo = MaxClipAmmo;
    LocalPosition = transform.localPosition;
    CurrentReloadTime = 0.0f;
    if(!AudioComp)
      AudioComp = gameObject.AddComponent<AudioSource>();
    AudioComp.clip = EmptyClipSound;
  }

  // Update is called once per frame
  protected void Update() {

    if(bIsReloading) {
      float CurveValue = ReloadCurve.Evaluate(CurrentReloadTime);
      transform.localPosition = LocalPosition + (ReloadOffset * CurveValue);
      CurrentReloadTime += Time.deltaTime;
      if(CurrentReloadTime >= ReloadTime) {
        CurrentReloadTime = 0.0f;
        bIsReloading = false;
        if(CurrentAmmo >= MaxClipAmmo) {
          CurrentClipAmmo = MaxClipAmmo;
          CurrentAmmo -= MaxClipAmmo;
        } else {
          CurrentClipAmmo = CurrentAmmo;
          CurrentAmmo = 0;
        }
      }
    }


    /*if(bIsReloading) {
      Vector3 position = Vector3.Lerp(LocalPosition, LocalPosition + ReloadOffset, ReloadTime * 0.5f);
      if(position == (LocalPosition + ReloadOffset)) {
        bIsReloading = false;
      }
    } else {
      Vector3 position = Vector3.Lerp(LocalPosition + ReloadOffset, LocalPosition, ReloadTime * 0.5f);
    }*/
  }

  public virtual bool Shoot() {
    if(bIsReloading) return false;
    if(CurrentClipAmmo <= 0) {
      Reload();
      return false;
    }
    CurrentClipAmmo -= 1;
    if(ShootSound)
      AudioComp.PlayOneShot(ShootSound);
    return true;
  }

  public void Reload() {
    if(CurrentAmmo == 0) {
      if(!AudioComp.isPlaying)
        AudioComp.Play();
        return;
    }
    if(CurrentClipAmmo == MaxClipAmmo) return;
    if(bIsReloading) return;
    if(ReloadSound)
      AudioComp.PlayOneShot(ReloadSound);
    bIsReloading = true;
    /*if(CurrentAmmo >= MaxClipAmmo) {
      CurrentClipAmmo = MaxClipAmmo;
      CurrentAmmo -= MaxClipAmmo;
    } else {
      CurrentClipAmmo = CurrentAmmo;
      CurrentAmmo = 0;
    }*/
  }

  public void RecoverAmmo(int ammo) {
    CurrentAmmo += ammo;
    if(CurrentAmmo > MaxAmmo) {
      CurrentAmmo = MaxAmmo;
    }
  }
}
