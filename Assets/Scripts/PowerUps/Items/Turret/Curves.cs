using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curves : MonoBehaviour {

    public Vector3[] points;

    public Transform target;
    public bool isOn;

    public void Reset()
    {
        //points = new Vector3[] { transform.localPosition, new Vector3(2f, 2f, 2f), new Vector3(3f, 3f, 3f) };
    }

    public Vector3 GetPoint(float t)
    {
        return Bezier.GetPoint(points[0], points[1], points[2],points[3], t);
    }

    public Vector3 GetVelocity(float t)
    {
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) -
            transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
}
