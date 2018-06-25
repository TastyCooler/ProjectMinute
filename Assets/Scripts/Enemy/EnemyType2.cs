using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : BaseEnemy {

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
        else if (enemyState == State.retreat)
        {
            KeepDistance();
        }
        else if (enemyState == State.searchingForPlayer)
        {
            SearchForPlayer();
        }
    }
}
