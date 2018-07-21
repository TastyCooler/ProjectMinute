using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour {

    #region Properties

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
    BaseEnemy baseEnemy;

    [SerializeField] float speed = 10f;
    [SerializeField] float despawnDelay;
    [SerializeField] float hookStrengthMultiplayer = 20f;
    float hookDuration;
    float despawnDelayMultiplayer = 1.5f;
    float hookStrenghPerDistance;
    bool hooked;
    
    float knockbackStrength;
    float knockbackDuration;

    GameObject owner;

    float timeWhenShot;

    #endregion

    #region Unity Messages

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
            hookStrenghPerDistance = hookStrengthMultiplayer * 11 / baseEnemy.Hit.distance;
            hookDuration = -0.06f * hookStrengthMultiplayer + 1.9f; // Linear Function
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
            collision.GetComponent<PlayerController>().TakeDamage(0, transform.up * (-knockbackStrength * hookStrenghPerDistance), Time.realtimeSinceStartup, despawnDelay / despawnDelayMultiplayer * hookDuration);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject.GetComponent<BaseEnemy>() && collision.gameObject != owner)
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage(0, transform.up * (-knockbackStrength * hookStrenghPerDistance), despawnDelay / despawnDelayMultiplayer * hookDuration);
            StartCoroutine(PushBackAfter(1f));
        }
        else if (collision.gameObject != owner)
        {
            StartCoroutine(PushBackAfter(1f));
        }
    }

    #endregion

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
