using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
   

   
    [SerializeField] GameObject arrow;
    // Use this for initialization
    void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {
        
       
	}

    void ShieldLogic()
    {


       GameObject Arrow = Instantiate(arrow, gameObject.transform.position, Quaternion.identity);
      // Arrow.transform.SetParent(null);
        
       

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("shield hit");


        ShieldLogic();
        
        
    }

}

