using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    PlayerController player;

    int timer; // shooting Timer
    [SerializeField] int shootingRate = 30; // shooting rate

    [SerializeField] int lifetime = 100; // Towers lifetime, get decreased per shot
    [SerializeField] float sightreach = 7f; // towers sight reach

    Vector3 enemypos;
    Vector3 toEnemy; // targets enemy

    [SerializeField] int turretDamage;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        Shoot();
    }

    void Shoot()
    {
        if (lifetime >= 0)
        {
            //TODO: Make it destroyable?
            //TODO: Implement another shoot mechanic?
            if (timer <= 0 && toEnemy.magnitude < sightreach)
            {
                ArrowController arrowToShoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
                arrowToShoot.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                arrowToShoot.Owner = player.gameObject;
                arrowToShoot.transform.up = toEnemy;
                arrowToShoot.Damage = turretDamage;

                timer = shootingRate;
                DecreaseLifeTime();
            }

            timer--;
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

    void DecreaseLifeTime()
    {
        lifetime--;
    }
}
