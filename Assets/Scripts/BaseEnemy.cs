using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour {

    [Header("Stats"), SerializeField] protected int attack = 1;
    [SerializeField] protected int health = 1;
    [SerializeField] protected float speed = 1f;

    protected Vector3 toPlayer;
    protected Vector3 playerLastSpottedAt;
    
    protected Vector3 targetPos;

    [SerializeField] GameObject[] powerupsToDrop;
    [Range(0, 1), SerializeField] float dropChance = 0.1f;

    protected PlayerController player;

    [SerializeField] protected float sightReach = 1f;
    [SerializeField] protected float patrolRadius = 1f;
    [SerializeField] protected float patrollingSpeedMultiplier = 0.3f;

    protected enum State
    {
        patrolling,
        playerSpotted,
        searchingForPlayer
    }
    protected State enemyState = State.patrolling;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    protected virtual void Update()
    {
        toPlayer = player.transform.position - transform.position;
    }

    protected virtual void Patrolling()
    {
        if (V3Equal(targetPos, Vector3.zero, 0.1f))
        {
            DefineNewTargetPos();
        }
        if (toPlayer.magnitude < sightReach)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightReach + 10f);
            if(hit.collider)
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    enemyState = State.playerSpotted;
                }
            }
        }
        if (!V3Equal(transform.position, targetPos, 0.1f))
        {
            transform.position += (targetPos - transform.position).normalized * (speed * patrollingSpeedMultiplier) * Time.deltaTime;
        }
        else
        {
            DefineNewTargetPos();
        }
    }

    protected virtual void PursuitPlayer()
    {
        transform.position += toPlayer.normalized * speed * Time.deltaTime;
        if (toPlayer.magnitude > sightReach)
        {
            enemyState = State.searchingForPlayer;
            playerLastSpottedAt = player.transform.position;
        }
    }

    protected virtual void SearchForPlayer()
    {
        if (toPlayer.magnitude < sightReach)
        {
            enemyState = State.playerSpotted;
        }
        transform.position += (playerLastSpottedAt - transform.position).normalized * speed * Time.deltaTime;
        if (V3Equal(transform.position, playerLastSpottedAt, 0.1f))
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

    // Check if two vectors are the same taking into account a small margin, in this case, the aimingtolerance
    protected bool V3Equal(Vector3 a, Vector3 b, float aimingTolerance)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }
}
