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

    [Header("Stats"), SerializeField] float speed = 1f;

    // Player Slots for item and skill
    BaseSkill playerSkill;
    BaseItem playerItem;

    Vector3 moveDirection;

    PlayerInput input;

    [SerializeField] GameObject cursor;

    
    private Rigidbody2D rb;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private Vector3 target;
    bool cd = false;

    public int combo;
    public float comboTime = 50f;

    public enum State
    {
        freeToMove,
        dashing,
        attacking
    }
    State playerState = State.dashing;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        target = transform.position;
        
    }

    private void Update()
    {
        if (playerState == State.freeToMove)
        {
            GetInput();
            transform.position += moveDirection * speed * Time.deltaTime;
            if (input.UseItem && playerItem)
            {
                playerItem.Use();
            }
            if (input.UseSkill && playerSkill)
            {
                playerSkill.Use();
            }
        }
        else if (playerState == State.attacking)
        {

        }
        else if (playerState == State.dashing)
        {

            print("I'm dashing");
            if (Input.GetMouseButtonDown(0))
            {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = transform.position.z;
                transform.position = Vector3.MoveTowards(transform.position, target + new Vector3(0.5f, 0.5f, 0f), dashSpeed);
            }
            if (dashTime <= 0)
            {
                dashTime = startDashTime;
                rb.velocity = Vector3.zero;

                
            } else
            {
                dashTime -= Time.deltaTime;
                
            }

            
        


            Debug.Log(dashTime);
            Combo();
        }
        cursor.transform.position = transform.position + moveDirection;
        Debug.LogFormat("X: {0} | Y: {1} | Z: {2}", moveDirection.x, moveDirection.y, moveDirection.z);
        //cursor.transform.localRotation.SetFromToRotation(cursor.transform.up, moveDirection);
        //cursor.transform.rotation.SetEulerAngles(cursor.transform.rotation.x, cursor.transform.rotation.y, );
        cursor.transform.rotation.SetLookRotation(moveDirection);

       
    }

    void Combo()
    {
        if (combo >= 1)
        {
            //Debug.Log(comboTime);
            Debug.Log(combo);
            comboTime -= Time.deltaTime;
            if (comboTime == 0)
            {
                comboTime = 0;
                combo -= 1;
            }
        }

        if (combo >= 3)
        {
            Special();
        }

    }

    void Special()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
           
            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.transform.position , dashSpeed);
        }
        Debug.DrawLine(transform.position, closestEnemy.transform.position);
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
