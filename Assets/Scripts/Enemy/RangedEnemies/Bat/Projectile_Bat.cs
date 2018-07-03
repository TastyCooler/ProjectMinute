using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bat : BaseProjectile {

    #region Properties

    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public float KnockbackStrength
    {
        get
        {
            return KnockbackStrength;
        }
        set
        {
            knockbackStrength = value;
        }
    }

    public float KnockbackDuration
    {
        get
        {
            return knockbackDuration;
        }
        set
        {
            knockbackDuration = value;
        }
    }

    public GameObject Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    #endregion

    protected PlayerController player;

    [SerializeField] float speed = 10f;
    [SerializeField] float despawnDelay = 3f;

    int damage;
    float knockbackStrength;
    float knockbackDuration;

    GameObject owner;

    float timeWhenShot;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeWhenShot = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup > timeWhenShot + despawnDelay)
        {
            BezierShoot();
            GameManager.Instance.PushLaser(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // TODO Make particle system explode
            collision.GetComponent<PlayerController>().TakeDamage(damage, transform.up * knockbackStrength, Time.realtimeSinceStartup, knockbackDuration);
            GameManager.Instance.PushLaser(gameObject);
        }
        else if (collision.gameObject != owner)
        {
            // TODO Make particle system explode
            GameManager.Instance.PushLaser(gameObject);
        }
    }

    protected override void BezierShoot()
    {
        if (done == false)
        {
            beziertime = beziertime + Time.deltaTime;
            transform.position += HelperMethods.CalculateQuadBezierPoint(beziertime, transform.position, player.transform.position, player.transform.position);
            if (beziertime > 1)
            {
                done = true;
                beziertime = 0;
            }
        }
    }
}
