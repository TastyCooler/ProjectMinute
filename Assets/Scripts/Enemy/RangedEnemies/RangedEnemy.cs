using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : BaseEnemy {

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
            Attack();
        }
        else if (enemyState == State.searchingForPlayer)
        {
            SearchForPlayer();
        }
        else if(enemyState == State.knockedBack)
        {
            GetKnockedBack();
        }
    }

    void Attack()
    {
        if (rangeAttacking)
        {
            if (Time.realtimeSinceStartup > timeWhenLastAttacked + attackCooldown)
            {
                anim.SetTrigger("Attack");
                timeWhenLastAttacked = Time.realtimeSinceStartup;

                ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
                arrowToShoot.Damage = attack;
                arrowToShoot.KnockbackDuration = knockbackDuration;
                arrowToShoot.KnockbackStrength = knockbackStrength;
                arrowToShoot.Owner = gameObject;
                arrowToShoot.transform.up = toPlayer;

                rangeAttacking = false;
            }
        }
    }
}
