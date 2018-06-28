using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

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

    protected PlayerController player;

    [SerializeField] float speed = 10f;
    [SerializeField] float despawnDelay = 3f;

    int damage;
    float knockbackStrength;
    float knockbackDuration;

    float timeWhenShot;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timeWhenShot = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        if(Time.realtimeSinceStartup > timeWhenShot + despawnDelay)
        {
            GameManager.Instance.PushArrow(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.PushArrow(gameObject);

        if (collision.tag == "Player")
        {
            GameManager.Instance.PushArrow(gameObject);
            // TODO Make particle system explode
            collision.GetComponent<PlayerController>().TakeDamage(damage, transform.up * knockbackStrength, Time.realtimeSinceStartup, knockbackDuration);
        }
    }
}
