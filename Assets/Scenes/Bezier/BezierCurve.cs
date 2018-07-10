using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {

    public Vector3[] points;

    public void Reset() //Inits the array with 3 points
    {
        points = new Vector3[]
        {
            new Vector3(1f,0f,0f),
            new Vector3(2f,0f,0f),
            new Vector3(3f,0f,0f),
            new Vector3(4f,0f,0f),
            new Vector3(5f,0f,0f)
        };
    }

    public Vector3 GetPoint (float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        //to make it unaffected by the position of the curve, we subtract that after transforming
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        // because using only GetVelocity instead of this method the lines are to long and clutter the view
        return GetVelocity(t).normalized;
    }
}
