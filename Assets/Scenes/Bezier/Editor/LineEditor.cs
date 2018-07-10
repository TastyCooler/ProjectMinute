using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Line))]
public class LineEditor : Editor {

	void OnSceneGUI()
    {
        Line line = target as Line; //this is the target variable which is set to be drawn
        Transform handleTransform = line.transform; // Handles. operates in world space, while the point are in the local space. This converts into world space
        Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity; // enables to choose between local and global rotation
        Vector3 p0 = handleTransform.TransformPoint(line.p0);
        Vector3 p1 = handleTransform.TransformPoint(line.p1);

        Handles.color = Color.white;
        Handles.DrawLine(p0, p1);

        EditorGUI.BeginChangeCheck(); // Handles(points) are enabled to move in scene
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point"); // This makes Undos possible
            EditorUtility.SetDirty(line); //? "Unity does not know that a change was made, so for example won't ask the user to save when quitting"
            line.p0 = handleTransform.InverseTransformPoint(p0); //handle values are in world space, this converts them back into lines local space
        }
        EditorGUI.BeginChangeCheck(); // this method checks if a change was made
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck()) // this method checks if the change is applied
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }
    }

}
