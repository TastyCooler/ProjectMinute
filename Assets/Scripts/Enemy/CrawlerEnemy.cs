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
            MeleeAttack();
        }
        else if (enemyState == State.searchingForPlayer)
        {
            SearchForPlayer();
        }
    }
}
