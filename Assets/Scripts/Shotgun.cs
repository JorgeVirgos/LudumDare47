﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {
    
  public int NumberOfPellets;
  public float DispersionAngle;
  public float DispersionDistance;

  private Transform child;

  // Start is called before the first frame update
  void Start() {
    CurrentAmmo = MaxAmmo;
    CurrentClipAmmo = MaxClipAmmo;
    child = transform.GetChild(0).transform;
  }

  // Update is called once per frame
  void Update() {
    CurrentShotCooldown -= Time.deltaTime;
  }

  public override void Shoot() {
    if(CurrentShotCooldown <= 0.0f) {
      base.Shoot();
      for(int i = 0; i < NumberOfPellets; ++i) {
        float yRotation = 25 * i - 50;
        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, yRotation, 0.0f));
        GameObject obj = Instantiate(Projectile, child.position, Quaternion.identity);
        if(obj) {
          BasicProjectile prj = obj.GetComponent<BasicProjectile>();
          if(prj) {
            Vector3 rand = Random.onUnitSphere /*+ transform.parent.transform.position*/ + 
              (transform.parent.transform.forward * DispersionDistance);
            rand.Normalize();
            prj.ImpulseDirection = rand + transform.parent.transform.forward;
          }
        }
      }
      CurrentShotCooldown = ShotCooldown;
      bIsShooting = true;
      if (CurrentClipAmmo <= 0) {
        Reload();
      }
    }
  }
}