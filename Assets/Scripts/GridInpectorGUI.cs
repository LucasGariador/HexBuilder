using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexGrid))]
public class GridInpectorGUI : Editor
{
    public override void OnInspectorGUI()
    {
        HexGrid hexGrid = (HexGrid)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Generate"))
        {
            hexGrid.LayoutGrid();
        }
    }
}
