using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    float beziertime;
    public Vector3 start;
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 end;
    bool done = false;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        BezierShoot();
        
    }
        Vector3 CalculateCubicBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 p2,Vector3 p3, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        float uuuu = uuu * u;
        float tttt = ttt * t;

        Vector3 p = uuuu * start; //first term
        p += 2 * uuu * t * p1; //second term
        p += 2 * uu * tt * p2; //third term
        p += 2 * u * ttt * p3;//fourth term
        p += tttt * end; //fifth term
        
        return p;
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 p2, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
       

        Vector3 p = uuu * start; //first term
        p += 3 * uu * t * p1; //second term
        p += 3 * u * tt * p2; //third term
        p += ttt * end; //fourth term

        return p;
    }

    Vector3 CalculateQuadBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;


        Vector3 p = uu * start; //first term
        p += 2 * u * t * p1; //second term
        p += tt * end; //third term

        return p;
    }

    void BezierShoot()
    {
        
        if (done == false)
        {
            beziertime = beziertime + Time.deltaTime;
            transform.position += CalculateCubicBezierPoint(beziertime, start, p0, p1, end);
            Debug.Log(beziertime);
            if (beziertime > 1)
            {
                done = true;
                beziertime = 0;
            }
        }
        
    }
}
