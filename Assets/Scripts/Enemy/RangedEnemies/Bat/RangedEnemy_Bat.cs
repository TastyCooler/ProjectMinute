using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy_Bat : BaseEnemy {

    protected override void Update()
    {
        base.Update();
        if (enemyState == State.patrolling)
        {
            Patrolling();
        }
        else if (enemyState == State.playerSpotted)
        {
            KeepDistance();
        }
        else if (enemyState == State.searchingForPlayer)
        {
            SearchForPlayer();
        }
    }

    protected override void RangeAttack()
    {
        Instantiate(projectile);
    }
}
