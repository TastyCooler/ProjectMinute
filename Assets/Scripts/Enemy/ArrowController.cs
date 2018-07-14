using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

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

    SpriteRenderer rend;
    BoxCollider2D coll;
    [SerializeField] ParticleSystem swooshParticle;
    ParticleSystem.EmissionModule swooshEmission;

    [SerializeField] ParticleSystem impactParticle;

    int damage;
    float knockbackStrength;
    float knockbackDuration;

    bool stop = false;

    GameObject owner;

    float timeWhenShot;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
        swooshEmission = swooshParticle.emission;
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeWhenShot = Time.realtimeSinceStartup;
        SetTraits(true);
    }

    private void Update()
    {
        if(!stop)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
        if(Time.realtimeSinceStartup > timeWhenShot + despawnDelay)
        {
            GameManager.Instance.PushArrow(gameObject);
            gameObject.layer = 11;  // EnemyProjectile layernumber = 11
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject != owner)
        {
            collision.GetComponent<PlayerController>().TakeDamage(damage, transform.up * knockbackStrength, Time.realtimeSinceStartup, knockbackDuration);
            impactParticle.Play();
            SetTraits(false);
            StartCoroutine(PushBackAfter(1f));
            gameObject.layer = 11;  // EnemyProjectile layernumber = 11
        }
        else if (collision.gameObject != owner)
        {
            impactParticle.Play();
            SetTraits(false);
            StartCoroutine(PushBackAfter(1f));
            gameObject.layer = 11;  // EnemyProjectile layernumber = 11
        }
    }

    void SetTraits(bool setTo)
    {
        rend.enabled = setTo;
        coll.enabled = setTo;
        stop = !setTo;
        if(setTo)
        {
            swooshEmission.rateOverDistance = 5f;
        }
        else
        {
            swooshEmission.rateOverDistance = 0f;
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
}
