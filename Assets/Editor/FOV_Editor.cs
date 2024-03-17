using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(EnemyFOV))]
public class FOV_Editor : Editor
{
    void OnSceneGUI()
    {
        EnemyFOV fov = (EnemyFOV)target;

        Vector3 fromAnglePos = fov.CirclePoint(-fov.viewAngle * 0.5f);
        Handles.color = Color.white;
        Handles.DrawWireDisc(fov.transform.position, Vector3.up, fov.viewRange);
        Handles.color = new Color(1f, 1f, 1f, 0.3f);
        Handles.DrawSolidArc(fov.transform.position, Vector3.up, fromAnglePos, fov.viewAngle, fov.viewRange);
        Handles.Label(fov.transform.position + (fov.transform.forward * 2.0f), fov.viewRange.ToString());

    }
}
