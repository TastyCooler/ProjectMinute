using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class i_Bow : BaseItem {
    
    [SerializeField, Range(0, 100)] float percentageToSlow; // Slow the player when hes shooting an arrow.
    float attackStartedTime;    // Save Time.realtimeSinceStartup in float var.
    float savePreviousSpeed;
    bool startTimer;
    bool alreadyShootArrow;

    public override void Use()
    {
        if (usageTimes > 0)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        startTimer = true;

        if (!alreadyShootArrow)
        {
            usageTimes--;

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
            if (Time.realtimeSinceStartup > attackStartedTime + cooldown)
            {
                player.Speed = savePreviousSpeed;
                startTimer = false;
                alreadyShootArrow = false;
            }
        }
    }
}
