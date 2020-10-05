using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
  public Vector3 ReloadRotation;
  public Vector3 ShootingOffset;
  public Vector3 ShootingRotation;
  public float ReloadTime;
  public AnimationCurve ReloadCurve;
  public AnimationCurve ShootingCurve;
  public float MinShootPitch;
  public float MaxShootPitch;
  public Text ClipAmmoText;
  public Text MaxAmmoText;

  internal WeaponType weaponType;

  protected int CurrentAmmo;
  protected int CurrentClipAmmo;
  protected float CurrentShotCooldown = 0.0f;
  protected float AmmunitionQuantity;
  protected float CurrentShootingTime;

  private AudioSource AudioComp;
  private bool bIsReloading;
  private Vector3 LocalPosition;
  private float CurrentReloadTime;
  private bool bIsCurrentWeapon;
  private bool bIsInAnimation;

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
    AudioComp.playOnAwake = false;
    UpdateAmmoUI();
    bIsCurrentWeapon = false;
  }

  // Update is called once per frame
  protected void Update() {
    float CurveValue = 0.0f;
    if(bIsInAnimation) {
      CurveValue = ShootingCurve.Evaluate(CurrentShootingTime);
      transform.localPosition = LocalPosition + (ShootingOffset * CurveValue);
      transform.localEulerAngles = ShootingRotation * CurveValue;
      CurrentShootingTime += Time.deltaTime;
    }
    if(CurrentShootingTime > ShotCooldown) {
      CurrentShootingTime = 0.0f;
      bIsInAnimation = false;
    }
    if(bIsReloading) {
      CurveValue = ReloadCurve.Evaluate(CurrentReloadTime);
      transform.localPosition = LocalPosition + (ReloadOffset * CurveValue);
      transform.localEulerAngles = ReloadRotation * CurveValue;
      CurrentReloadTime += Time.deltaTime;
      if(CurrentReloadTime >= ReloadTime) {
        CurrentReloadTime = 0.0f;
        bIsReloading = false;
        if(CurrentAmmo >= MaxClipAmmo) {
          CurrentAmmo -= MaxClipAmmo - CurrentClipAmmo;
          CurrentClipAmmo = MaxClipAmmo;
        } else {
          CurrentClipAmmo += CurrentAmmo;
          if(CurrentClipAmmo > MaxClipAmmo) {
            CurrentAmmo = CurrentClipAmmo - MaxClipAmmo;
            CurrentClipAmmo = MaxClipAmmo;
          } else {
            CurrentAmmo = 0;
          }
        }
        UpdateAmmoUI();
      }
    }
  }

  public virtual bool Shoot() {
    if(bIsReloading) return false;
    if(CurrentClipAmmo <= 0) {
      Reload();
      return false;
    }
    CurrentClipAmmo -= 1;
    bIsInAnimation = true;
    UpdateAmmoUI();
    if (ShootSound) {
      float rand = Random.Range(MinShootPitch, MaxShootPitch);
      AudioComp.pitch = rand;
      AudioComp.PlayOneShot(ShootSound);
    }
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
  }

  public void RecoverAmmo(int ammo) {
    CurrentAmmo += ammo;
    if(CurrentAmmo > MaxAmmo) {
      CurrentAmmo = MaxAmmo;
    }
    UpdateAmmoUI();
  }

  public void UpdateAmmoUI() {
    if(bIsCurrentWeapon) {
      ClipAmmoText.text = CurrentClipAmmo.ToString();
      MaxAmmoText.text = CurrentAmmo.ToString();
    }
  }

  public void SetCurrentWeapon(bool current) {
    bIsCurrentWeapon = current;
  }

}
