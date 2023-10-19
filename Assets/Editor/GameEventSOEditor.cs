using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameEventSO))]
public class GameEventSoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameEventSO gameEventSO = (GameEventSO)target;
        
        if(GUILayout.Button("Raise"))
        {
            gameEventSO.Raise();
        }
    }
}