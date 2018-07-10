using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Curves))]
public class CurveInspector : Editor {

    private Curves curve;
    PlayerController player;

    private static int lineSteps = 8;

   

    private void OnSceneGUI()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        curve = target as Curves;
        curve.transform.position = player.transform.position;
        if (curve.isOn)
        {


            Vector3 p0 = curve.points[0];
            Vector3 p1 = curve.points[1];
            Vector3 p2 = curve.points[2];
            Vector3 p3 = curve.points[3];

            //Setting first point on transform.position
            curve.points[0] =   player.gameObject.transform.position;

            //Setting second point in middle of the first and third
            Vector3 center = (curve.transform.position + curve.target.position) / 2f;
            center.y = center.y + 20f;
            curve.points[1] = center;

            //Setting fourth point on target
            curve.points[3] = curve.target.position;

            //Draw curve
            Vector3 lineStart = curve.GetPoint(0f);
            Handles.color = Color.green;
            Handles.DrawLine(lineStart, lineStart + curve.GetDirection(0f));
            for (int i = 1; i <= lineSteps; i++)
            {
                Vector3 lineEnd = curve.GetPoint(i / (float)lineSteps);
                Handles.color = Color.white;
                Handles.DrawLine(lineStart, lineEnd);
                Handles.color = Color.green;
                Handles.DrawLine(lineEnd, lineEnd + curve.GetDirection(i / (float)lineSteps));
                lineStart = lineEnd;
            }
        }
    }
}

