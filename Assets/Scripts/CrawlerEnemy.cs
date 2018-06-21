using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerEnemy : BaseEnemy {

    protected override void Update()
    {
        base.Update();
        if(enemyState == State.patrolling)
        {
            if(toPlayer.magnitude < sightReach)
            {
                enemyState = State.playerSpotted;
            }
            if(!V3Equal(transform.position, startPos, 0.1f))
            {
                transform.position += (startPos - transform.position).normalized * speed * Time.deltaTime;
            }
            // TODO patroll around
        }
        else if(enemyState == State.playerSpotted)
        {
            transform.position += toPlayer.normalized * speed * Time.deltaTime;
            if(toPlayer.magnitude > sightReach)
            {
                enemyState = State.searchingForPlayer;
                playerLastSpottedAt = player.transform.position;
            }
        }
        else if(enemyState == State.searchingForPlayer)
        {
            if(toPlayer.magnitude < sightReach)
            {
                enemyState = State.playerSpotted;
            }
            transform.position += (playerLastSpottedAt - transform.position).normalized * speed * Time.deltaTime;
            if(V3Equal(transform.position, playerLastSpottedAt, 0.1f))
            {
                enemyState = State.patrolling;
            }

        }
    }

    // Check if two vectors are the same taking into account a small margin, in this case, the aimingtolerance
    bool V3Equal(Vector3 a, Vector3 b, float aimingTolerance)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }
}
