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
    BaseEnemy baseEnemy;

    [SerializeField] float speed = 10f;
    [SerializeField] float despawnDelay;

    int damage;
    float knockbackStrength;
    float knockbackDuration;
    [SerializeField] float playerLaserDamageMultiplayer;

    GameObject owner;

    float timeWhenShot;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseEnemy = FindObjectOfType<BaseEnemy>();
        timeWhenShot = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        //despawnDelay = baseEnemy.Hit.distance / speed;

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
        else if (collision.gameObject != owner)
        {
            StartCoroutine(PushBackAfter(1f));
        }
    }

    IEnumerator PushBackAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (gameObject.layer != 11)
        {
            gameObject.layer = 11; // EnemyProjectile layernumber = 11, this gonna reset layer to enemies projectile after shoot from Player.
        }
        GameManager.Instance.PushLaser(gameObject);
    }
}
