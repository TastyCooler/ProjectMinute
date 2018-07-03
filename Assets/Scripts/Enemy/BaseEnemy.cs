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

    protected bool attacking;
    [SerializeField] protected float attackDistance = 1f;

    protected Animator anim;

    protected enum State
    {
        patrolling,
        playerSpotted,
        searchingForPlayer
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
        float newTargetPos = 0;
        newTargetPos += 1;

        if (HelperMethods.V3Equal(targetPos, Vector3.zero, 0.1f))
        {
            DefineNewTargetPos();
        }
        if (toPlayer.magnitude < sightReach)
        {
            //enemyState = State.playerSpotted;
            hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightReach + 10f, hitLayer);
            if (hit.collider != null)
            {
                if  (hit.collider.gameObject.tag == "Player")
                {
                    enemyState = State.playerSpotted;
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

        if (newTargetPos >= 30)
        {
            newTargetPos = 0;
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

        if(toPlayer.magnitude < attackDistance && !attacking)
        {
            MeleeAttack();
        }

        if(Time.realtimeSinceStartup > timeWhenLastAttacked + attackDuration + attackCooldown)
        {
            attacking = false;
        }

        if (!attacking)
        {
            transform.position += toPlayer.normalized * speed * Time.deltaTime;
        }
    }

    protected virtual void MeleeAttack()
    {
        attacking = true;
        timeWhenLastAttacked = Time.realtimeSinceStartup;
        anim.SetTrigger("Attack");
    }

    protected virtual void RangeAttack()
    {
        timeWhenLastAttacked = Time.realtimeSinceStartup;
        ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
        arrowToShoot.Damage = attack;
        arrowToShoot.KnockbackDuration = knockbackDuration;
        arrowToShoot.KnockbackStrength = knockbackStrength;
        arrowToShoot.Owner = gameObject;
        arrowToShoot.transform.up = toPlayer;
    }

    protected virtual void KeepDistance()
    {
        if (toPlayer.magnitude < sightReach * sightReachMultiplier)
        {
            transform.position += -toPlayer.normalized * speed * Time.deltaTime;
        }

        if (toPlayer.magnitude > sightReach * sightReachMultiplier && toPlayer.magnitude < sightReach * (sightReachMultiplier + attackAreaTolerance))
        {
            if(Time.realtimeSinceStartup > timeWhenLastAttacked + attackCooldown)
            {
                RangeAttack();
            }
        }

        if (toPlayer.magnitude > sightReach * (sightReachMultiplier + attackAreaTolerance) && toPlayer.magnitude < sightReach)
        {
            transform.position += toPlayer.normalized * speed * Time.deltaTime;
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
        transform.position += (playerLastSpottedAt - transform.position).normalized * speed * Time.deltaTime;
        if (HelperMethods.V3Equal(transform.position, playerLastSpottedAt, 0.1f))
        {
            enemyState = State.patrolling;
            DefineNewTargetPos();
        }
    }

	public void TakeDamage(int damage, Vector3 knockback)
    {
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
