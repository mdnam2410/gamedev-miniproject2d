using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class AssetPreviewExporter : MonoBehaviour
{
    public void Export()
    {
        Texture2D texture = AssetPreview.GetAssetPreview(gameObject.transform);
        byte[] textureData = texture.EncodeToPNG();

        File.WriteAllBytes($"Assets/{gameObject.name}.png", textureData);
    }
}
