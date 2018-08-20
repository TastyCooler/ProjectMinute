using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackHitboxController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            BossController ownerController = transform.parent.GetComponent<BossController>();
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(ownerController.Attack, collision.gameObject.transform.position - transform.parent.transform.position, Time.realtimeSinceStartup, ownerController.PlayerKnockedBackFor);
        }
    }
}
