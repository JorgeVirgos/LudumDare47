using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon {

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
    if (CurrentShotCooldown <= 0.0f) {
      base.Shoot();
      Instantiate(Projectile, child.position, Quaternion.identity);
      BasicProjectile prj = Projectile.GetComponent<BasicProjectile>();
      if (prj) prj.ImpulseDirection = transform.parent.transform.forward;
      CurrentShotCooldown = ShotCooldown;
      bIsShooting = true;
      if(CurrentClipAmmo <= 0) {
        Reload();
      }
    }
  }
}
