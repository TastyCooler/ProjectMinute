﻿using System.Collections;
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

    [SerializeField]protected GameObject projectile;

    protected PlayerController player;

    [SerializeField] protected float sightReach = 1f;
    [SerializeField] protected float patrolRadius = 1f;
    [SerializeField] protected float patrollingSpeedMultiplier = 0.3f;

    [Range(0.1f, 1), SerializeField] float sightReachMultiplier = 0.5f;
    [Range(0.1f, 0.3f), SerializeField] float attackAreaTolerance = 3f;

    [SerializeField] int highscoreValue;

    protected enum State
    {
        patrolling,
        playerSpotted,
        searchingForPlayer
    }
    protected State enemyState = State.patrolling;

    RaycastHit2D hit;
    public LayerMask hitLayer;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        int playerLayer = LayerMask.NameToLayer("Player");
        int obstacleLayer = LayerMask.NameToLayer("Obstacles");
        hitLayer = 1 << playerLayer;
        LayerMask obsLayer = 1 << obstacleLayer;
        hitLayer = hitLayer | obsLayer;
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
    }

    protected virtual void PursuitPlayer()
    {
        transform.position += toPlayer.normalized * speed * Time.deltaTime;
        hit = Physics2D.Raycast(transform.position, toPlayer.normalized, sightReach + 10f, hitLayer);

        if (hit.collider.gameObject.tag != "Player")
        {
            enemyState = State.patrolling;
        }

        if (toPlayer.magnitude > sightReach)
        {
            enemyState = State.searchingForPlayer;
            playerLastSpottedAt = player.transform.position;
        }
    }

    protected virtual void RangeAttack()
    {
        
    }

    protected virtual void MeleeAttack()
    {

    }

    protected virtual void KeepDistance()
    {
        if (toPlayer.magnitude < sightReach * sightReachMultiplier)
        {
            transform.position += -toPlayer.normalized * speed * Time.deltaTime;
        }

        if (toPlayer.magnitude > sightReach * sightReachMultiplier && toPlayer.magnitude < sightReach * (sightReachMultiplier + attackAreaTolerance))
        {
            RangeAttack(); //TODO: Enemy Shoots only on collision enter?
            //TODO: Adding a Reaction Time!
        }

        if (toPlayer.magnitude > sightReach * (sightReachMultiplier + attackAreaTolerance) && toPlayer.magnitude < sightReach)
        {
            transform.position += toPlayer.normalized * speed * Time.deltaTime;
        }

        if (toPlayer.magnitude > sightReach)
        {
            enemyState = State.searchingForPlayer;
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
