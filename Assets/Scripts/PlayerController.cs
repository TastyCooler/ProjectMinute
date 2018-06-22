﻿using System.Collections;
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

    public int Attack
    {
        get
        {
            return attack;
        }
    }

    public float KnockbackStrength
    {
        get
        {
            return knockbackStrength;
        }
    }

    [Header("Stats"), SerializeField] float speed = 1f;

    // Player Slots for item and skill
    BaseSkill playerSkill;
    BaseItem playerItem;

    Vector3 moveDirection;
    Vector3 aimDirection;

    Vector3 velocity;

    Animator anim;

    

    [SerializeField] int baseAttack = 3;
    int attack;
    float attackStartedTime;
    [SerializeField] float attackDuration = 0.5f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float knockbackStrength = 1f;
    [SerializeField] int baseHealth = 5;
    int health;

    PlayerInput input;

    Camera cam;

    [SerializeField] GameObject cursor;

    public enum State
    {
        freeToMove,
        dashing,
        attacking,
        knockedBack
    }
    State playerState = State.freeToMove;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        cam = Camera.main;
        anim = GetComponent<Animator>();

        attack = baseAttack;
        health = baseHealth;
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
            if(input.Attack && Time.realtimeSinceStartup > attackStartedTime + attackDuration + attackCooldown)
            {
                playerState = State.attacking;
                anim.SetTrigger("Attack");
                attackStartedTime = Time.realtimeSinceStartup;
            }
        }
        else if(playerState == State.attacking)
        {
            if(Time.realtimeSinceStartup > attackStartedTime + attackDuration)
            {
                playerState = State.freeToMove;
            }
        }
        else if(playerState == State.dashing)
        {
            print("I'm dashing");
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + aimDirection);
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
        if(GameManager.Instance.IsControllerInput)
        {
            if(!HelperMethods.V3Equal(moveDirection, Vector3.zero, 0.1f))
            {
                aimDirection = moveDirection;
            }
        }
        else
        {
            Vector3 mousePosInWorld = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetAim = (mousePosInWorld - transform.position);
            aimDirection.x = targetAim.normalized.x;
            aimDirection.y = targetAim.normalized.y;
        }
        SetArrow();
    }

    // Set the arrow position and rotation
    void SetArrow()
    {
        cursor.transform.position = transform.position + aimDirection.normalized;
        cursor.transform.rotation = Quaternion.FromToRotation(cursor.transform.up, aimDirection) * cursor.transform.rotation;
    }

    // Subtracts damage from the player health and knocks him back
    public void TakeDamage(int damage, Vector3 knockback)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
        playerState = State.knockedBack;
        // TODO make player take damage
    }

    void Die()
    {
        //TODO make the player die and open gameover menu
    }

    // Collects Items or Skills if player walks over them if nothing is equipped before
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<BaseItem>() && !playerItem)
        {
            collision.GetComponent<BaseItem>().Equipped = true;
            playerItem = collision.GetComponent<BaseItem>();
        }
        else if(collision.GetComponent<BaseSkill>() && !playerSkill)
        {
            collision.GetComponent<BaseSkill>().Equipped = true;
            playerSkill = collision.GetComponent<BaseSkill>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<BaseItem>())
        {
            if (input.SwitchPowerup && playerItem)
            {
                playerItem.Equipped = false;
                collision.GetComponent<BaseItem>().Equipped = true;
                playerItem = collision.GetComponent<BaseItem>();
            }
        }
        else if (collision.GetComponent<BaseSkill>())
        {
            if(input.SwitchPowerup && playerSkill)
            {
                playerSkill.Equipped = false;
                collision.GetComponent<BaseSkill>().Equipped = true;
                playerSkill = collision.GetComponent<BaseSkill>();
            }
        }
    }
}
