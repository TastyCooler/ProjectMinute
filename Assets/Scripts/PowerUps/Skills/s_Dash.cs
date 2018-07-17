using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Dash : BaseSkill {

    [Header("Skills"), SerializeField] float dashDuration = 0.5f;
    float timeWhenDashStarted;

    public override void Use()
    {
        if(Time.realtimeSinceStartup > timeWhenDashStarted + dashDuration + cooldown)
        {
            player.PlayerState = PlayerController.State.dashing;
            timeWhenDashStarted = Time.realtimeSinceStartup;
            StartCoroutine(EndDash(dashDuration));
        }
    }

    IEnumerator EndDash(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        player.PlayerState = PlayerController.State.freeToMove;
    }
}
