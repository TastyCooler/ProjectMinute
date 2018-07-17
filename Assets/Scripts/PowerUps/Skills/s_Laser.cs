using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Laser : BaseSkill {

    protected override void RunFunctionalityOfItem()
    {
        base.RunFunctionalityOfItem();
        ThrowHook();
    }

    void ThrowHook()
    {
        Projectile_Bat laserToShoot = GameManager.Instance.GetLaser(transform.position).GetComponent<Projectile_Bat>();
        laserToShoot.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        laserToShoot.Owner = player.gameObject;
        laserToShoot.transform.up = player.AimDirection;
    }
}