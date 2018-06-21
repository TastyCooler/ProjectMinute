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

	public void TakeDamage(int damage, Vector3 knockback)
    {
        health -= damage;
        if(health <= 0)
        {
            // TODO kill the enemy
        }
    }
}
