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

    #region Private Fields

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
    [SerializeField] float playerArrowDamageMultiplayer;

    bool stop = false;
    bool deflected = false;

    GameObject owner;

    float timeWhenShot;

    #endregion

    #region Unity Messages

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
        if(!stop && !deflected)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        if (!stop && deflected)
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
            transform.position += transform.up * speed * Time.deltaTime;
        }

        if(Time.realtimeSinceStartup > timeWhenShot + despawnDelay)
        {
            StartCoroutine(PushBackAfter(0));
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
        }
        else if (collision.gameObject.GetComponent<BaseEnemy>() && collision.gameObject != owner)
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage((int)(player.Attack * playerArrowDamageMultiplayer), (collision.gameObject.transform.position - transform.position).normalized * player.KnockbackStrength, knockbackDuration);
            impactParticle.Play();
            SetTraits(false);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject.GetComponent<PlayerAttackHitBoxController>() || collision.gameObject.layer == LayerMask.NameToLayer("Shield"))
        {
            deflected = true;
            transform.up = player.AimDirection;
        }
        else if (collision.gameObject.GetComponent<BaseEnemy>() && deflected)
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage((int)(player.Attack * playerArrowDamageMultiplayer), (collision.gameObject.transform.position - transform.position).normalized * player.KnockbackStrength, knockbackDuration);
            impactParticle.Play();
            SetTraits(false);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject != owner)
        {
            impactParticle.Play();
            SetTraits(false);
            StartCoroutine(PushBackAfter(1f));
        }
    }

    #endregion

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
        yield return new WaitForSeconds(seconds);
        if (gameObject.layer != LayerMask.NameToLayer("EnemyProjectile"))
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }
        deflected = false;
        GameManager.Instance.PushArrow(gameObject);
    }
}
