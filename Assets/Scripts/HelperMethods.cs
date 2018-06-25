using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperMethods : MonoBehaviour {

    // Check if two vectors are the same taking into account a small margin, in this case, the aimingtolerance
    public static bool V3Equal(Vector3 a, Vector3 b, float aimingTolerance)
    {
        return Vector3.SqrMagnitude(a - b) < aimingTolerance;
    }

}
