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
    float despawnDelay;
    float despawnDelayMultiplayer = 1.5f;


    int damage = 0;
    float knockbackStrength;
    float knockbackDuration;

    GameObject owner;

    float timeWhenShot;

    public float DespawnDelay
    {
        get { return despawnDelay; }
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseEnemy = FindObjectOfType<BaseEnemy>();
        timeWhenShot = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        despawnDelay = baseEnemy.Hit.distance / speed * despawnDelayMultiplayer;

        if (Time.realtimeSinceStartup > timeWhenShot + despawnDelay)
        {
            GameManager.Instance.PushHook(gameObject);
            SetLayer(11);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != owner)
        {
            // TODO Make particle system explode
            collision.GetComponent<PlayerController>().TakeDamage(0, transform.up * (-knockbackStrength * hookStrengthMultiplayer), Time.realtimeSinceStartup, despawnDelay / despawnDelayMultiplayer + hookDuration);
            StartCoroutine(PushBackAfter(1f));
            SetLayer(11);
        }
        else if (collision.gameObject != owner)
        {
            // TODO Make particle system explode
            StartCoroutine(PushBackAfter(1f));
            SetLayer(11);
        }
    }

    IEnumerator PushBackAfter(float seconds)
    {
        // I know, hard coded stuff isn´t the nicest way x).
        if (gameObject.layer != 11)
        {
            gameObject.layer = 11; // EnemyProjectile layernumber = 11, this gonna reset layer to enemies projectile after shoot from Player.
        }
        yield return new WaitForSeconds(seconds);
        GameManager.Instance.PushArrow(gameObject);
    }

    private void SetLayer(int layerNumber)
    {
        gameObject.layer = layerNumber;
    }
}
