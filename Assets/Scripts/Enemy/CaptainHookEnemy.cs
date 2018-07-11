using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainHookEnemy : BaseEnemy {
    
    float savedSpeed;
    bool hooking = false;
    [SerializeField] float hookCooldown;

    protected override void Awake()
    {
        base.Awake();
        savedSpeed = speed;
    }

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
        if (toPlayer.magnitude < attackDistance && !meleeAttacking)
        {
            meleeAttacking = true;
            timeWhenLastAttacked = Time.realtimeSinceStartup;
            anim.SetTrigger("Attack");
            speed = savedSpeed;
        }

        if (toPlayer.magnitude > attackDistance && toPlayer.magnitude < sightReach && !hooking)
        {
            timeWhenLastAttacked = Time.realtimeSinceStartup;
            meleeAttacking = false;
            hooking = true;
            speed = 0;
        }

        if (hooking && !meleeAttacking && Time.realtimeSinceStartup > timeWhenLastAttacked + attackDuration + attackCooldown + hookCooldown)
        {
            timeWhenLastAttacked = Time.realtimeSinceStartup;

            HookController hookToShoot = GameManager.Instance.GetHook(transform.position).GetComponent<HookController>();
            hookToShoot.Damage = attack;
            hookToShoot.KnockbackDuration = knockbackDuration;
            hookToShoot.KnockbackStrength = knockbackStrength;
            hookToShoot.Owner = gameObject;
            hookToShoot.transform.up = toPlayer;

            hooking = false;
            meleeAttacking = true;
        }

        if (toPlayer.magnitude > sightReach)
        {
            speed = savedSpeed;
        }
    }
}
