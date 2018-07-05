using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Turret : BaseItem {

    [SerializeField] int usages = 1;
    [SerializeField] GameObject turret;
    public override void Use()
    {
        if (usageTimes <= usages)
        {
            usages--;
            Spawn();
        }
    }


    void Spawn()
    {
        GameObject tower = Instantiate(turret, gameObject.transform);
       //  ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
       // arrowToShoot.Owner = player.gameObject;
       // arrowToShoot.transform.up = FindObjectOfType<BaseEnemy>().transform.position;
    }
}
