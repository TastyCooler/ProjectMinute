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
    [SerializeField] float hookStrengthMultiplayer;
    [SerializeField] float hookDuration;
    [SerializeField] float despawnDelay;
    float despawnDelayMultiplayer = 1.5f;


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

        if (gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            despawnDelay = 0.5f;
        }

        if (gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            despawnDelay = baseEnemy.Hit.distance / speed * despawnDelayMultiplayer;
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
            collision.GetComponent<PlayerController>().TakeDamage(0, transform.up * (-knockbackStrength * hookStrengthMultiplayer), Time.realtimeSinceStartup, despawnDelay / despawnDelayMultiplayer + hookDuration);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject.GetComponent<BaseEnemy>() && collision.gameObject != owner)
        {
            Debug.Log("HookEnemy");
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage(0, transform.up * (-knockbackStrength * hookStrengthMultiplayer), despawnDelay / despawnDelayMultiplayer + hookDuration);
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
        GameManager.Instance.PushHook(gameObject);
    }
}
