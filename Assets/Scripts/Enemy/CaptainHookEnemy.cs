using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainHookEnemy : BaseEnemy {
    
    [SerializeField] float hookCooldown;

    protected override void Update()
    {
        base.Update();
        if (enemyState == State.patrolling)
        {
            Patrolling();
        }
        else if (enemyState == State.playerSpotted)
        {
            PursuitPlayer();
            Attack();
        }
        else if (enemyState == State.searchingForPlayer)
        {
            SearchForPlayer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Time.realtimeSinceStartup < timeWhenLastAttacked + attackDuration)
        {
            collision.GetComponent<PlayerController>().TakeDamage(attack, toPlayer * knockbackStrength, Time.realtimeSinceStartup, knockbackDuration);
        }
    }

    void Attack()
    {
        if (toPlayer.magnitude < sightReach && toPlayer.magnitude > attackDistance)
        {
            if (Time.realtimeSinceStartup > timeWhenLastAttacked + attackDuration + attackCooldown + hookCooldown)
            {
                meleeAttacking = true;
                // TODO: Play animation here!
                timeWhenLastAttacked = Time.realtimeSinceStartup;

                HookController hookToShoot = GameManager.Instance.GetHook(transform.position).GetComponent<HookController>();
                hookToShoot.Damage = attack;
                hookToShoot.KnockbackDuration = knockbackDuration;
                hookToShoot.KnockbackStrength = knockbackStrength;
                hookToShoot.Owner = gameObject;
                hookToShoot.transform.up = toPlayer;
            }
        }
    }
}
