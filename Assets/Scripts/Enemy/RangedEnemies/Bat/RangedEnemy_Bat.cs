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
                timeWhenLastAttacked = Time.realtimeSinceStartup;

                Projectile_Bat laserToShoot = GameManager.Instance.GetLaser(transform.position).GetComponent<Projectile_Bat>();
                laserToShoot.Damage = attack;
                laserToShoot.KnockbackDuration = knockbackDuration;
                laserToShoot.KnockbackStrength = knockbackStrength;
                laserToShoot.Owner = gameObject;
                laserToShoot.transform.up = toPlayer;

                rangeAttacking = false;
            }
        }
    }
}
