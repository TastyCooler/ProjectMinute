using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Laser : BaseSkill {

    protected override void RunFunctionalityOfItem()
    {
        base.RunFunctionalityOfItem();
        Shoot();
    }

    void Shoot()
    {
        Projectile_Bat laserToShoot = GameManager.Instance.GetLaser(transform.position).GetComponent<Projectile_Bat>();
        laserToShoot.gameObject.layer = 13; // PlayerProjectile layernumber = 13.
        laserToShoot.Owner = player.gameObject;
        laserToShoot.transform.up = player.AimDirection;
    }
}