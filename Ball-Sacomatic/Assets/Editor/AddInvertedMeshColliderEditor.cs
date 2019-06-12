using UnityEngine;
using UnityEditor;
using System.Collections;

//Script taken from https://forum.unity.com/threads/can-you-invert-a-sphere-or-box-collider.118733/
[CustomEditor(typeof(AddInvertedMeshCollider))]
public class AddInvertedMeshColliderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AddInvertedMeshCollider script = (AddInvertedMeshCollider)target;
        if (GUILayout.Button("Create Inverted Mesh Collider"))
            script.CreateInvertedMeshCollider();
    }
}