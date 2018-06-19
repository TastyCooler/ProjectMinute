using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    [Header("Stats"), SerializeField] float speed = 1f;

    // Player Slots for item and skill
    BaseSkill playerSkill;
    BaseItem playerItem;

    Vector3 moveDirection;

    PlayerInput input;

    enum State
    {
        freeToMove,
        dashing,
        attacking
    }
    State playerState = State.freeToMove;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if(playerState == State.freeToMove)
        {
            GetInput();
            transform.position += moveDirection * speed * Time.deltaTime;
            if(input.UseItem && playerItem)
            {
                playerItem.Use();
            }
            if(input.UseSkill && playerSkill)
            {
                playerSkill.Use();
            }
        }
        else if(playerState == State.attacking)
        {

        }
        else if(playerState == State.dashing)
        {

        }
    }

    void GetInput()
    {
        moveDirection.x = input.Horizontal;
        if(moveDirection.x < 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(moveDirection.x > 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        moveDirection.y = input.Vertical;
    }

}
