using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Dash : BaseSkill {

    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void Use()
    {
        player.PlayerState = PlayerController.State.dashing;
    }
}
