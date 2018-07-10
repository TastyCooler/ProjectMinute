using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : BaseItem {

    [SerializeField] GameObject projectile;

    public override void Use()
    {
        Instantiate(projectile, player.transform.position, Quaternion.identity);
    }
}
