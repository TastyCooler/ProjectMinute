using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [SerializeField] PlayerController pC;

    int shieldHealth = 5;
     public int ShieldHealth {
        get { return shieldHealth; }
        set { shieldHealth = value; }
    }


	void Update () {
        ShieldLogic();
	}

    void ShieldLogic()
    {
        
        if(ShieldHealth <= 0)
        {
            pC = FindObjectOfType<PlayerController>();
            pC.Invincible = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {   
            ShieldHealth--;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            ShieldHealth--;
        }
    }
}