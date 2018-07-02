using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour {

    protected float beziertime; // The time determines how long the projectile travels
    [SerializeField] protected Vector3 start; // first point
    [SerializeField] protected Vector3 p0; // second point
    [SerializeField] protected Vector3 p1; // third point
    [SerializeField] protected Vector3 p2; // fourth point 
    [SerializeField] protected Vector3 end; // fifth point. More can be added!
    protected bool done = false; // Stops the Projectile from looping 

    // Update is called once per frame
    void Update()
    {
        BezierShoot();
        
    }
     
    // Prefixed Projectile Logic
    protected virtual void BezierShoot()
    {
        
        if (done == false)
        {
            beziertime = beziertime + Time.deltaTime;
            this.transform.position += HelperMethods.CalculateCubicBezierPoint(beziertime, start, p0, p1, end);
            Debug.Log(beziertime);
            if (beziertime > 1)
            {
                done = true;
                beziertime = 0;
            }
        }
        
    }
}
