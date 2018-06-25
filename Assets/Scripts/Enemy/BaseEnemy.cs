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

    [SerializeField] protected float distanceToKeep = 1f;
    float saveSpeed;

    protected enum State
    {
        patrolling,
        playerSpotted,
        retreat,
        searchingForPlayer
    }
    protected State enemyState = State.patrolling;

    RaycastHit2D hit;
    public LayerMask hitLayer;

    protected virtual void Awake()
    {
        saveSpeed = speed;

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
    }

    private void OnDrawGizmos()
    {
        if (toPlayer.magnitude < sightReach)
        {
            Debug.DrawRay(transform.position, toPlayer.normalized * (sightReach + 10f));
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

        if (toPlayer.magnitude < distanceToKeep)
        {
            enemyState = State.retreat;
        }
    }

    protected virtual void KeepDistance()
    {
        if (toPlayer.magnitude < distanceToKeep)
        {
            transform.position -= toPlayer.normalized * speed * Time.deltaTime;
        }
        else if (toPlayer.magnitude < distanceToKeep + 1)
        {
            speed = 0;
        }
        else
        {
            speed = saveSpeed;
        }

        if (toPlayer.magnitude > distanceToKeep)
        {
            enemyState = State.playerSpotted;
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
