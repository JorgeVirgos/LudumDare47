﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockAttackGun : Weapon{
    public int NumberOfPellets;
    public float DispersionAngle;
    public float DispersionDistance;

    private Transform child;
    // Start is called before the first frame update
    void Start()
    {
        CurrentAmmo = MaxAmmo;
        CurrentClipAmmo = MaxClipAmmo;
        //child = transform.GetChild(0).transform;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentShotCooldown -= Time.deltaTime;
    }
    public override bool Shoot()
    {
        if (CurrentShotCooldown <= 0.0f)
        {
            if(!base.Shoot()) return false;
            GameObject obj = Instantiate(Projectile, child.position, Quaternion.identity);
            if (obj)
            {
                BasicProjectile prj = obj.GetComponent<BasicProjectile>();
                if (prj)
                {
                    prj.Damage = WeaponDamage;
                    prj.ImpulseDirection = transform.parent.transform.forward;
                }
                CurrentShotCooldown = ShotCooldown;
                bIsShooting = true;
                if (CurrentClipAmmo <= 0)
                {
                    Reload();
                }
            }
        }
        return true;
    }
    
}
