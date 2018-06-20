using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    PlayerController pC;
    

    private void Awake()
    {
        pC = GetComponent<PlayerController>();
        pC = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        pC.combo++;
    }
  
    
}
