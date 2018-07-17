using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This gets attached to the player item slot
/// </summary>
public class BaseItem : PowerUpFloating {

    [SerializeField] protected int usageTimes = 1;
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
        if (usageTimes >= 0)
        {
            if (!alreadyUsingItem)
            {
                savePreviousSpeed = player.Speed;
                StartCoroutine(CastUsage());
            }
        }
    }

    protected virtual void RunFunctionalityOfItem()
    {
        startTimer = true;

        if (!alreadyUsingItem)
        {
            usageTimes--;
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

        if (usageTimes == 0)
        {
            Destroy(gameObject);
        }
    }
}
