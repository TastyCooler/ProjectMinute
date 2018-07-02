using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods : MonoBehaviour {

    // Check if two vectors are the same taking into account a small margin, in this case, the aimingtolerance
    public static bool V3Equal(Vector3 a, Vector3 b, float aimingTolerance)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }


    /// <summary>
    /// Calculates a quartic Bezier Curve
    /// </summary>
    /// <param name="t">The time determines how long the projectile travels</param>
    /// <param name="start">First Point</param>
    /// <param name="p1">Second Point</param>
    /// <param name="p2">Third Point</param>
    /// <param name="p3">Fourth Point</param>
    /// <param name="end">Fifth Point</param>
    /// <returns></returns>
    public static Vector3 CalculateQuarticBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 end)
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

    /// <summary>
    /// Calculates a cubic Bezier Curve
    /// </summary>
    /// <param name="t">The time determines how long the projectile travels</param>
    /// <param name="start">First Point</param>
    /// <param name="p1">Second Point</param>
    /// <param name="p2">Third Point</param>
    /// <param name="p3">Fourth Point</param>
    /// <param name="end">Fifth Point</param>
    /// <returns></returns>
    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 p2, Vector3 end)
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

    /// <summary>
    /// Calculates a quad Bezier Curve
    /// </summary>
    /// <param name="t">The time determines how long the projectile travels</param>
    /// <param name="start">First Point</param>
    /// <param name="p1">Second Point</param>
    /// <param name="p2">Third Point</param>
    /// <param name="p3">Fourth Point</param>
    /// <param name="end">Fifth Point</param>
    /// <returns></returns>
    public static Vector3 CalculateQuadBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;


        Vector3 p = uu * start; //first term
        p += 2 * u * t * p1; //second term
        p += tt * end; //third term

        return p;
    }



}
