using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitBoxController : MonoBehaviour {

    PlayerController player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<BaseEnemy>())
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage((int)(player.Attack * player.AttackMultiplier), (collision.gameObject.transform.position - transform.position).normalized * player.KnockbackStrength);
        }
    }

}
