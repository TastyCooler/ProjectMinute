using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour {

    public State PlayerState
    {
        get
        {
            return playerState;
        }
        set
        {
            playerState = value;
        }
    }

    public GameObject Cursor
    {
        get
        {
            return cursor;
        }
    }

    [Header("Stats"), SerializeField] float speed = 1f;

    // Player Slots for item and skill
    BaseSkill playerSkill;
    BaseItem playerItem;

    Vector3 moveDirection;

    PlayerInput input;

    [SerializeField] GameObject cursor;

    public enum State
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
            print("I'm dashing");
        }
        cursor.transform.position = transform.position + moveDirection;
        cursor.transform.localRotation = Quaternion.FromToRotation(cursor.transform.up, moveDirection) * cursor.transform.localRotation;
        //cursor.transform.rotation.SetEulerAngles(cursor.transform.rotation.x, cursor.transform.rotation.y, );
        //cursor.transform.rotation.SetLookRotation(moveDirection);
        //cursor.transform.rotation = Quaternion.Euler();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BaseItem>())
        {
            collision.GetComponent<BaseItem>().Equipped = true;
            playerItem = collision.GetComponent<BaseItem>();
        }
        else if(collision.GetComponent<BaseSkill>())
        {
            collision.GetComponent<BaseSkill>().Equipped = true;
            playerSkill = collision.GetComponent<BaseSkill>();
        }
    }

}
