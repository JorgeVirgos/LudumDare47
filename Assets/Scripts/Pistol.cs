using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon {

  private Transform child;

  Pistol() {
    weaponType = WeaponType.kWeaponTypePistol;
  }

  // Start is called before the first frame update
  void Start() {
    CurrentAmmo = MaxAmmo;
    CurrentClipAmmo = MaxClipAmmo;
    child = transform.GetChild(0).transform;
  }

  // Update is called once per frame
  void Update() {
    base.Update();
    CurrentShotCooldown -= Time.deltaTime;
  }

  public override bool Shoot() {
    if(CurrentShotCooldown <= 0.0f || !bIsShooting) {
      if(!base.Shoot()) return false;
      GameObject obj = Instantiate(Projectile, child.position, Quaternion.identity);
      if(obj) {
        BasicProjectile prj = obj.GetComponent<BasicProjectile>();
        if(prj) {
          CurrentShootingTime = 0.0f;
          prj.Damage = WeaponDamage;
          prj.Direction = transform.parent.forward;
          prj.ImpulseDirection = transform.parent.transform.forward;
        } else {
          EnemyProjectile eprj = obj.GetComponent<EnemyProjectile>();
          if (eprj)  {
            CurrentShootingTime = 0.0f;
            eprj.Damage = WeaponDamage;
            eprj.Direction = transform.parent.forward;
            eprj.ImpulseDirection = transform.parent.transform.forward;
          }
        }
        CurrentShotCooldown = ShotCooldown;
        bIsShooting = true;
      }
    }
    return true;
  }
}
