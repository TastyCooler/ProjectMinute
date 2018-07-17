using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Hook : BaseSkill {

    protected override void RunFunctionalityOfItem()
    {
        base.RunFunctionalityOfItem();
        Shoot();
    }

    void Shoot()
    {
        HookController hookToThrow = GameManager.Instance.GetHook(transform.position).GetComponent<HookController>();
        hookToThrow.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        hookToThrow.Owner = player.gameObject;
        hookToThrow.transform.up = player.AimDirection;
    }
}
