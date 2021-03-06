﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {
    
  public int NumberOfPellets;
  public float DispersionAngle;
  public float DispersionDistance;

  private Transform child;

  Shotgun() {
    weaponType = WeaponType.kWeaponTypeShotgun;
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
    if(CurrentShotCooldown <= 0.0f && !bIsShooting) {
      if(!base.Shoot()) return false;
      for(int i = 0; i < NumberOfPellets; ++i) {
        float yRotation = 25 * i - 50;
        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, 0.0f));
        GameObject obj = Instantiate(Projectile, child.position, Quaternion.identity);
        if(obj) {
          BasicProjectile prj = obj.GetComponent<BasicProjectile>();
          if(prj) {
            prj.Damage = WeaponDamage;
            prj.Direction = transform.parent.forward;
            Vector3 rand = Random.onUnitSphere + 
              (transform.parent.transform.forward * DispersionDistance);
            rand.Normalize();
            prj.ImpulseDirection = rand + transform.parent.transform.forward;
          } else {
          EnemyProjectile eprj = obj.GetComponent<EnemyProjectile>();
          if (eprj)  {
            eprj.Damage = WeaponDamage;
            eprj.Direction = transform.parent.forward;
            Vector3 rand = Random.onUnitSphere + 
              (transform.parent.transform.forward * DispersionDistance);
            rand.Normalize();
            eprj.ImpulseDirection = rand + transform.parent.transform.forward;
          }
        }
        }
      }
      CurrentShotCooldown = ShotCooldown;
      bIsShooting = true;
    }
    return true;
  }
}
