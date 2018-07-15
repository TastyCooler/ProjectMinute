using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Bow : BaseItem {
    
    [SerializeField] int usages = 20;
    [SerializeField] float shootCooldown;   // How long the player cannot move and shoot cannot shoot shoot another arrow.
    [SerializeField, Range(0, 100)] float percentageToSlow; // Slow the player when hes shooting an arrow.
    float attackStartedTime;    // Save Time.realtimeSinceStartup in float var.
    float savePreviousSpeed;
    bool startTimer;
    bool alreadyShootArrow;

    public override void Use()
    {
        if(usageTimes <= usages)
        {
            usages--;
            Shoot();
        }
    }

    void Shoot()
    {
        startTimer = true;

        if (!alreadyShootArrow)
        {
            player.GetInput();
            attackStartedTime = Time.realtimeSinceStartup;
            savePreviousSpeed = player.Speed;
            player.Speed = player.Speed - (percentageToSlow * player.Speed / 100);

            ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
            arrowToShoot.gameObject.layer = 13; // PlayerProjectile layernumber = 13.
            arrowToShoot.Owner = player.gameObject;
            arrowToShoot.transform.up = player.AimDirection;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (startTimer)
        {
            alreadyShootArrow = true;
            if (Time.realtimeSinceStartup > attackStartedTime + shootCooldown)
            {
                player.Speed = savePreviousSpeed;
                startTimer = false;
                alreadyShootArrow = false;
            }
        }
    }
}
