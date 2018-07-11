using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour {

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

    int damage = 0;
    float knockbackStrength;
    float knockbackDuration;

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
        despawnDelay = baseEnemy.Hit.distance / speed * 1.5f;

        if (Time.realtimeSinceStartup > timeWhenShot + despawnDelay)
        {
            GameManager.Instance.PushHook(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != owner)
        {
            // TODO Make particle system explode
            collision.GetComponent<PlayerController>().TakeDamage(0, transform.up * (-knockbackStrength), Time.realtimeSinceStartup, baseEnemy.Hit.distance / 5 * (knockbackDuration * 2));
            GameManager.Instance.PushHook(gameObject);
        }
        else if (collision.gameObject != owner)
        {
            // TODO Make particle system explode
            GameManager.Instance.PushHook(gameObject);
        }
    }
}
