using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Bow : BaseItem {
    
    protected override void RunFunctionalityOfItem()
    {
        base.RunFunctionalityOfItem();
        Shoot();
    }

    void Shoot()
    {
        ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
        arrowToShoot.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        arrowToShoot.Owner = player.gameObject;
        arrowToShoot.transform.up = player.AimDirection;
    }
}
