using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerEnemy : BaseEnemy {

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

    void Attack()
    {
        if (toPlayer.magnitude < attackDistance && !meleeAttacking)
        {
            meleeAttacking = true;
            timeWhenLastAttacked = Time.realtimeSinceStartup;
            anim.SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && Time.realtimeSinceStartup < timeWhenLastAttacked + attackDuration)
        {
            collision.GetComponent<PlayerController>().TakeDamage(attack, toPlayer * knockbackStrength, Time.realtimeSinceStartup, knockbackDuration);
        }
    }
}
