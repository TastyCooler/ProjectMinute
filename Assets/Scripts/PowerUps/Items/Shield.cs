using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    PlayerController player;
    BaseEnemy enemy;
    ArrowController arrows;
    Projectile_Bat batArrows;
    GameManager gM;
    int shieldHealth = 10;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        
        
    }

	void Update () {
        ShieldLogic();
        Debug.Log(shieldHealth);
	}

    void ShieldLogic()
    {
        if(shieldHealth <= 0)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("shield hit");

        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        if (collision.gameObject.tag == "Enemy")
        {   
           shieldHealth--;
        }
        if (collision.gameObject.tag == "Projectile")
        {
            arrows = GameObject.FindGameObjectWithTag("Projectile").GetComponent<ArrowController>();
            Destroy(arrows);
            shieldHealth--;
        }
        if (collision.gameObject.tag == "Projectile")
        {
            // THOSE FKIN LASERS ARE TOO FAST
            batArrows = GameObject.FindGameObjectWithTag("Projectile").GetComponent<Projectile_Bat>();
            Destroy(batArrows);
            shieldHealth--;
        }

    }

}

