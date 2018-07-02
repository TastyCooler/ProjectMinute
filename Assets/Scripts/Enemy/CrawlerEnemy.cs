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
}
