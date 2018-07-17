using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bat : MonoBehaviour {

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
    [SerializeField] float despawnDelay;

    int damage;
    float knockbackStrength;
    float knockbackDuration;
    [SerializeField] float playerLaserDamageMultiplayer;

    GameObject owner;
    bool deflected = false;

    float timeWhenShot;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeWhenShot = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (!deflected)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        if (deflected)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
            transform.position += transform.up * speed * Time.deltaTime;
        }

        if (Time.realtimeSinceStartup > timeWhenShot + despawnDelay)
        {
            StartCoroutine(PushBackAfter(0));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != owner)
        {
            // TODO Make particle system explode
            collision.GetComponent<PlayerController>().TakeDamage(damage, transform.up * knockbackStrength, Time.realtimeSinceStartup, knockbackDuration);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject.GetComponent<BaseEnemy>() && collision.gameObject != owner)
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage((int)(player.Attack * playerLaserDamageMultiplayer), (collision.gameObject.transform.position - transform.position).normalized * player.KnockbackStrength, knockbackDuration);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            deflected = true;
            transform.up = player.AimDirection;
        }
        else if (collision.gameObject.GetComponent<BaseEnemy>() && deflected)
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage((int)(player.Attack * playerLaserDamageMultiplayer), (collision.gameObject.transform.position - transform.position).normalized * player.KnockbackStrength, knockbackDuration);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject != owner)
        {
            StartCoroutine(PushBackAfter(1f));
        }
    }

    IEnumerator PushBackAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (gameObject.layer != LayerMask.NameToLayer("EnemyProjectile"))
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
        deflected = false;
        GameManager.Instance.PushLaser(gameObject);
    }
}