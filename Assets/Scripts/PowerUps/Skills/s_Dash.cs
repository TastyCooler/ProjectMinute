using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_Dash : BaseSkill {

    PlayerController player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //startPos = transform.position; // gets the starting position of the object
        //rend = GetComponent<SpriteRenderer>();
    }

    public override void Use()
    {
        player.PlayerState = PlayerController.State.dashing;
    }
}
