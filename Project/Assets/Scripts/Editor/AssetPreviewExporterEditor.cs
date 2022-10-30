using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssetPreviewExporter))]
public class AssetPreviewExporterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AssetPreviewExporter script = (AssetPreviewExporter)target;

        if (GUILayout.Button("Export"))
        {
            script.Export();
        }
    }
}
