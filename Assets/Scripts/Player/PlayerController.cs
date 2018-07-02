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

    public float AttackMultiplier
    {
        get
        {
            return attackMultiplier;
        }
    }

    [Header("Stats"), SerializeField] float speed = 1f;

    // Player Slots for item and skill
    BaseSkill playerSkill;
    BaseItem playerItem;

    Vector3 moveDirection;
    Vector3 lastValidMoveDir;
    Vector3 aimDirection;

    Vector3 velocity;

    Animator anim;

    

    [SerializeField] int baseAttack = 3;
    int attack;
    float attackMultiplier = 1f;
    float attackStartedTime;
    [SerializeField] float attackDuration = 0.5f;
    [SerializeField] float attackTwoDuration = 0.5f;
    [Range(1, 5), SerializeField] float attackTwoDamageMultiplier = 1.5f;
    [SerializeField] float attackThreeDuration = 0.5f;
    [Range(1, 10), SerializeField] float attackThreeDamageMultiplier = 2f;
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float knockbackStrength = 1f;
    [SerializeField] int baseHealth = 5;
    int health;
    bool keepAttacking = false;

    [SerializeField] float dashForce = 1f;

    PlayerInput input;

    Camera cam;

    [SerializeField] GameObject cursor;

    public GameObject projectile;
    
  

    public enum State
    {
        freeToMove,
        dashing,
        attacking,
        attackingTwo,
        attackingThree,
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
            velocity = moveDirection * speed;
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
                keepAttacking = false;
                playerState = State.attacking;
                attackMultiplier = 1f;
                anim.SetTrigger("Attack");
                attackStartedTime = Time.realtimeSinceStartup;
            }
        }
        else if(playerState == State.attacking)
        {
            GetInput();
            velocity = moveDirection * speed;
            if (input.Attack)
            {
                keepAttacking = true;
            }
            if(Time.realtimeSinceStartup > attackStartedTime + attackDuration && !keepAttacking)
            {
                playerState = State.freeToMove;
            }
            else if (Time.realtimeSinceStartup > attackStartedTime + attackDuration && keepAttacking)
            {
                keepAttacking = false;
                playerState = State.attackingTwo;
                attackMultiplier = attackTwoDamageMultiplier;
                anim.SetTrigger("AttackTwo");
                attackStartedTime = Time.realtimeSinceStartup;
            }
        }
        else if(playerState == State.attackingTwo)
        {
            GetInput();
            velocity = moveDirection * speed;
            if (input.Attack)
            {
                keepAttacking = true;
            }
            if (Time.realtimeSinceStartup > attackStartedTime + attackTwoDuration && !keepAttacking)
            {
                playerState = State.freeToMove;
            }
            else if (Time.realtimeSinceStartup > attackStartedTime + attackTwoDuration && keepAttacking)
            {
                playerState = State.attackingThree;
                attackMultiplier = attackThreeDamageMultiplier;
                anim.SetTrigger("AttackThree");
                attackStartedTime = Time.realtimeSinceStartup;
            }
        }
        else if(playerState == State.attackingThree)
        {
            GetInput();
            velocity = moveDirection * speed;
            if (Time.realtimeSinceStartup > attackStartedTime + attackThreeDuration)
            {
                playerState = State.freeToMove;
            }
        }
        else if(playerState == State.dashing)
        {
            velocity = lastValidMoveDir.normalized * dashForce;
            // TODO Set the dash animation
        }
        transform.position += velocity * Time.deltaTime;
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
        // Only overwrite lastValidMoveDir if the player is not standing still. To always dash in a direction
        if(!HelperMethods.V3Equal(moveDirection, Vector3.zero, 0.1f))
        {
            lastValidMoveDir = moveDirection;
        }
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
