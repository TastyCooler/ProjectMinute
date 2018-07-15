using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    PlayerController player;

    [SerializeField] int timer; // The Shootrate
    const int initialtimer = 30; // Used to reset timer

    [SerializeField] int lifetime = 100; // Towers lifetime, get decreased per shot

    [SerializeField] float sightreach = 7f; // towers sight reach 
    Vector3 enemypos;
    Vector3 toEnemy; // targets enemy

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        timer = initialtimer;
    }

    private void Update()
    {
        if(lifetime >= 0)
        {
            //TODO: Make it destroyable?
            //TODO: Implement another shoot mechanic?
            Shoot();
        }

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        enemypos = FindObjectOfType<BaseEnemy>().transform.position;
        toEnemy = enemypos - transform.position;
    }

    void Shoot()
    {
        if(timer <= 0  && toEnemy.magnitude < sightreach)
        {
            ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
            arrowToShoot.gameObject.layer = 13; // PlayerProjectile layernumber = 13 ----> make damage on enemies.
            arrowToShoot.Owner = player.gameObject;
            arrowToShoot.transform.up = toEnemy;

            timer = initialtimer;
            DecreaseLifeTime();
        }

        timer--;
    }

    void DecreaseLifeTime()
    {
        lifetime--;
    }
}
