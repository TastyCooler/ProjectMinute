using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    PlayerController player;

    [SerializeField] int shootingRate = 30; // shooting rate
    [SerializeField] int lifetime = 100; // Towers lifetime, get decreased per shot
    [SerializeField] float sightreach = 7f; // towers sight reach
    [SerializeField] int turretDamage;
    int timer; // shooting Timer

    Vector3 enemypos;
    Vector3 toEnemy; // targets enemy
    List<BaseEnemy> EnemyList;
    List<Transform> EnemyToTransform;
    
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        EnemyList = new List<BaseEnemy>();
        EnemyToTransform = new List<Transform>();
        AddAllEnemies(EnemyList);

        foreach (BaseEnemy enemy in EnemyList)
        {
            EnemyToTransform.Add(enemy.transform);
        }
    }

    void Update()
    {
        Shoot();
    }

    // TODO: Delete destroyed GameObject from list.
    // TODO: Stop searching for enemy outside of sightreach

    void Shoot()
    {
        // Quick and dirty. This calls "try" and if theres gonna be an ErrorException it calls "catch"
        try
        {
            enemypos = GetClosestEnemy(EnemyToTransform).transform.position;
            toEnemy = enemypos - transform.position;
        }
        catch
        {
            EnemyList.Clear();
            EnemyToTransform.Clear();
            AddAllEnemies(EnemyList);

            foreach (BaseEnemy enemy in EnemyList)
            {
                EnemyToTransform.Add(enemy.transform);
            }
        }

        // Or. Go to BaseEnemy script and return something if Die() gets called

        if (lifetime > 0 && EnemyToTransform.Count > 0)
        {
            //TODO: Make it destroyable?
            //TODO: Implement another shoot mechanic?

            if (timer <= 0 && toEnemy.magnitude < sightreach)
            {
                ArrowController arrowtoshoot = GameManager.Instance.GetArrow(transform.position).GetComponent<ArrowController>();
                arrowtoshoot.gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                arrowtoshoot.Owner = player.gameObject;
                arrowtoshoot.transform.up = toEnemy;
                arrowtoshoot.Damage = turretDamage;

                timer = shootingRate;
                lifetime--;
            }

            timer--;
        }

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

        Debug.LogFormat("EnemyList: {0}, EnemyToTransform: {1}", EnemyList.Count, EnemyToTransform.Count);
    }

    /// <summary>
    /// This iterates through the assigned Transform List and search for the nearest Object.
    /// </summary>
    /// <param name="newTransforms">You need to save the transforms from the GameObjects of GameObject List to Transform List!</param>
    /// <returns></returns>
    Transform GetClosestEnemy(List<Transform> newTransforms)
    {
        Transform bestTarget = null;
        Vector3 currentPosition = transform.position;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Transform potentialTarget in newTransforms)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    /// <summary>
    /// This adds all Objects with a specific script attached to it to this List.
    /// </summary>
    /// <param name="newList">You need a List where you can save the Objects into it.</param>
    void AddAllEnemies(List<BaseEnemy> newList)
    {
        BaseEnemy[] enemies = FindObjectsOfType(typeof(BaseEnemy)) as BaseEnemy[];

        foreach (BaseEnemy enemy in enemies)
        {
            newList.Add(enemy);
        }
    }
}
