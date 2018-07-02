using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bat : BaseProjectile {

    protected override void BezierShoot()
    {
         
        if (done == false)
        {
            beziertime = beziertime + Time.deltaTime;
            transform.position += HelperMethods.CalculateQuadBezierPoint(beziertime, start, p0, end);
            Debug.Log(beziertime);
            if (beziertime > 1)
            {
                done = true;
                beziertime = 0;
            }
        }
    }


}
