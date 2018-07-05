using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Bow : BaseItem {

    [SerializeField] int usages = 20;
    public override void Use()
    {
        if(usageTimes <= usages)
        {
            usages--;
            Debug.Log("Bow used");
            Shoot();
        }
    }

    void Shoot()
    {
        ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
        arrowToShoot.Owner = player.gameObject;
        arrowToShoot.transform.up = player.AimDirection;
    }
}
