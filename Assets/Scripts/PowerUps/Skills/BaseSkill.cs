using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This gets attached to the player skill slot
/// </summary>
public class BaseSkill : PowerUpFloating {
    
    [SerializeField] protected float casttime = 1;
    [SerializeField] protected float cooldown = 0.3f;
    [SerializeField, Range(0, 100)] protected float percentageToSlow; // Slow the player when hes shooting an arrow.
    protected float attackStartedTime;
    protected float savePreviousSpeed;
    protected bool startTimer;
    protected bool alreadyUsingItem;

    // Activate the item and make use of its benefits
    public virtual void Use()
    {
        if (!alreadyUsingItem)
        {
            savePreviousSpeed = player.Speed;
            StartCoroutine(CastUsage());
        }
    }

    protected virtual void RunFunctionalityOfItem()
    {
        startTimer = true;

        if (!alreadyUsingItem)
        {
            StartCoroutine(EndShootingArrow());
            attackStartedTime = Time.realtimeSinceStartup;
            player.Speed = player.Speed - (percentageToSlow * player.Speed / 100);
        }
    }

    protected override void Update()
    {
        base.Update();
        if (startTimer)
        {
            alreadyUsingItem = true;
            if (Time.realtimeSinceStartup > attackStartedTime + cooldown)
            {
                player.Speed = savePreviousSpeed;
                startTimer = false;
                alreadyUsingItem = false;
            }
        }
    }

    IEnumerator CastUsage()
    {
        player.PlayerState = PlayerController.State.usingItemOrSkill;
        player.Speed = player.Speed - (percentageToSlow * player.Speed / 100);
        yield return new WaitForSeconds(casttime);
        player.Speed = savePreviousSpeed;
        RunFunctionalityOfItem();
    }

    IEnumerator EndShootingArrow()
    {
        yield return new WaitForSeconds(cooldown);
        player.PlayerState = PlayerController.State.freeToMove;
    }
}
