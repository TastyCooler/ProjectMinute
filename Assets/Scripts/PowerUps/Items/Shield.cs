using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    
    [SerializeField] int shieldHealth = 5;

	void Update () {
        ShieldLogic();
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {   
            shieldHealth--;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            shieldHealth--;
        }
    }
}