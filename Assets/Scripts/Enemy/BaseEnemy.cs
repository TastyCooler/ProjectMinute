using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    [Header("Stats"), SerializeField] protected int attack = 1;
    [SerializeField] protected float knockbackDuration = 1f;
    [SerializeField] protected float knockbackStrength = 1f;
    [SerializeField] protected int health = 1;
    [SerializeField] protected float speed = 1f;

    protected Vector3 toPlayer;
    protected Vector3 playerLastSpottedAt;
    
    protected Vector3 targetPos;

    [SerializeField] protected GameObject[] powerupsToDrop;
    [Range(0, 1), SerializeField] protected float dropChance = 0.1f;

    protected PlayerController player;

    [SerializeField] protected float sightReach = 1f;
    [SerializeField] protected float patrolRadius = 1f;
    [SerializeField] protected float patrollingSpeedMultiplier = 0.3f;

    [Range(0.1f, 1), SerializeField] protected float sightReachMultiplier = 0.5f;
    [Range(0.1f, 0.3f), SerializeField] protected float attackAreaTolerance = 3f;

    [SerializeField] protected int highscoreValue;
    [SerializeField] protected int expToGive = 3;

    protected float timeWhenLastAttacked;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float attackDuration = 0.4f;

    protected bool meleeAttacking;
    protected bool rangeAttacking;
    [SerializeField] protected float attackDistance = 1f;

    protected Animator anim;
    private float newTargetPosTimer = 0;

    public RaycastHit2D Hit
    {
        get { return hit; }
    }

    protected enum State
    {
        patrolling,
        playerSpotted,
        searchingForPlayer,
        knockedBack
    }
    protected State enemyState = State.patrolling;

    protected RaycastHit2D hit;
    protected LayerMask hitLayer;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        int playerLayer = LayerMask.NameToLayer("Player");
        int obstacleLayer = LayerMask.NameToLayer("Obstacles");
        hitLayer = 1 << playerLayer;
        LayerMask obsLayer = 1 << obstacleLayer;
        hitLayer = hitLayer | obsLayer;
        anim = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        toPlayer = player.transform.position - transform.position;
        toPlayer.z = 0f;

        if (newTargetPosTimer <= 100)
        {
            newTargetPosTimer += 1;
        }
    }

    private void OnDrawGizmos()
    {
        if (toPlayer.magnitude < sightReach)
        {
            Debug.DrawRay(transform.position, toPlayer.normalized * toPlayer.magnitude);
        }
    }

    protected virtual void Patrolling()
    {
        if (HelperMethods.V3Equal(targetPos, Vector3.zero, 0.1f))
        {
            DefineNewTargetPos();
        }
        if (toPlayer.magnitude < sightReach)
        {
            hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightReach + 10f, hitLayer);
            if (hit.collider != null)
            {
                if  (hit.collider.gameObject.tag == "Player")
                {
                    enemyState = State.playerSpotted;
                    timeWhenLastAttacked = Time.realtimeSinceStartup;
                }
            }
        }
        if (!HelperMethods.V3Equal(transform.position, targetPos, 0.1f))
        {
            transform.position += (targetPos - transform.position).normalized * (speed * patrollingSpeedMultiplier) * Time.deltaTime;
        }
        else
        {
            DefineNewTargetPos();
        }

        if (newTargetPosTimer >= 100)
        {
            newTargetPosTimer = 0;
            DefineNewTargetPos();
        }
    }

    protected virtual void PursuitPlayer()
    {
        hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightReach + 10f, hitLayer);
        if (hit.collider.gameObject.tag != "Player")
        {
            playerLastSpottedAt = player.transform.position;
            enemyState = State.searchingForPlayer;
        }

        if (toPlayer.magnitude > sightReach)
        {
            playerLastSpottedAt = player.transform.position;
            enemyState = State.searchingForPlayer;
        }

        if(toPlayer.magnitude < attackDistance && !meleeAttacking)
        {
            MeleeAttack();
        }

        if(Time.realtimeSinceStartup > timeWhenLastAttacked + attackDuration + attackCooldown)
        {
            meleeAttacking = false;
        }

        if (!meleeAttacking)
        {
            transform.position += toPlayer.normalized * speed * Time.deltaTime;
        }
    }

    protected virtual void MeleeAttack()
    {
        meleeAttacking = true;
        timeWhenLastAttacked = Time.realtimeSinceStartup;
        anim.SetTrigger("Attack");
    }

    protected virtual void KeepDistance()
    {
        // Go away from player
        if (toPlayer.magnitude < sightReach * sightReachMultiplier && !rangeAttacking)
        {
            transform.position += -toPlayer.normalized * speed * Time.deltaTime;
            timeWhenLastAttacked = Time.realtimeSinceStartup;
            rangeAttacking = false;
        }

        // Go near to player
        if (toPlayer.magnitude > sightReach * (sightReachMultiplier + attackAreaTolerance) && toPlayer.magnitude < sightReach && !rangeAttacking)
        {
            transform.position += toPlayer.normalized * speed * Time.deltaTime;
            timeWhenLastAttacked = Time.realtimeSinceStartup;
            rangeAttacking = false;
        }

        // Attacking Player
        if (toPlayer.magnitude > sightReach * sightReachMultiplier && toPlayer.magnitude < sightReach * (sightReachMultiplier + attackAreaTolerance))
        {
            rangeAttacking = true;
        }

            if (toPlayer.magnitude > sightReach)
        {
            enemyState = State.searchingForPlayer;
            playerLastSpottedAt = player.transform.position;
        }

        hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightReach + 10f, hitLayer);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag != "Player")
            {
                enemyState = State.searchingForPlayer;
                playerLastSpottedAt = player.transform.position;
            }
        }
    }

    protected virtual void SearchForPlayer()
    {
        hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightReach + 10f, hitLayer);

        if (toPlayer.magnitude < sightReach)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                enemyState = State.playerSpotted;
            }
        }
        newTargetPosTimer -= 2;
        //Debug.Log(newTargetPosTimer);
        transform.position += (playerLastSpottedAt - transform.position).normalized * speed * Time.deltaTime;
        if (HelperMethods.V3Equal(transform.position, playerLastSpottedAt, 0.1f))
        {
            enemyState = State.patrolling;
            DefineNewTargetPos();
        }
        if (newTargetPosTimer <= 0)
        {
            enemyState = State.patrolling;
            DefineNewTargetPos();
        }
    }

	public void TakeDamage(int damage, Vector3 knockback)
    {
        // TODO apply knockback
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        DropPowerup();
        GameManager.Instance.Highscore += highscoreValue;
        player.GainExp(expToGive);
        Destroy(gameObject);
    }

    protected void DropPowerup()
    {
        if(Random.value <= dropChance)
        {
            Instantiate(powerupsToDrop[Random.Range(0, powerupsToDrop.Length - 1)], transform.position, transform.rotation);
        }
    }

    protected void DefineNewTargetPos()
    {
        Vector2 relativePoint = Random.insideUnitCircle * patrolRadius;
        targetPos = transform.position + new Vector3(relativePoint.x, relativePoint.y, 0f);
    }
}
