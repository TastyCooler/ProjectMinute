using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHitBoxController : MonoBehaviour {

    PlayerController player;

    [SerializeField] ParticleSystem hitSlash;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BaseEnemy>())
        {
            Vector3 toCollision = collision.transform.position - transform.position;
            hitSlash.transform.position = new Vector3((transform.position + (toCollision * 0.5f)).x, (transform.position + (toCollision * 0.5f)).y);
            hitSlash.Play();
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage((int)(player.Attack * player.AttackMultiplier), (collision.gameObject.transform.position - transform.position).normalized * player.KnockbackStrength);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.GetComponent<BaseEnemy>())
    //    {
    //        ContactPoint2D contact = collision.contacts[0];
    //        hitSlash.transform.position = contact.point;
    //        hitSlash.Play();
    //        collision.gameObject.GetComponent<BaseEnemy>().TakeDamage((int)(player.Attack * player.AttackMultiplier), (collision.gameObject.transform.position - transform.position).normalized * player.KnockbackStrength);
    //    }
    //}

}
