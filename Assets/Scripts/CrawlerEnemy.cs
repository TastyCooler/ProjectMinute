using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlerEnemy : BaseEnemy {

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Update()
    {
        base.Update();
        if(enemyState == State.patrolling)
        {
            if(V3Equal(targetPos, Vector3.zero, 0.1f))
            {
                DefineNewTargetPos();
            }
            if(toPlayer.magnitude < sightReach)
            {
                enemyState = State.playerSpotted;
            }
            if(!V3Equal(transform.position, targetPos, 0.1f))
            {
                transform.position += (targetPos - transform.position).normalized * (speed * patrollingSpeedMultiplier) * Time.deltaTime;
            }
            else
            {
                DefineNewTargetPos();
            }
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
                DefineNewTargetPos();
            }

        }
    }

    void DefineNewTargetPos()
    {
        Vector2 relativePoint = Random.insideUnitCircle * patrolRadius;
        targetPos = transform.position + new Vector3(relativePoint.x, relativePoint.y, 0f);

    }

    // Check if two vectors are the same taking into account a small margin, in this case, the aimingtolerance
    bool V3Equal(Vector3 a, Vector3 b, float aimingTolerance)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }
}
