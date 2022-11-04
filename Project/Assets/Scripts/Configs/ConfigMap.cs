using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigMapData
{
    public string Name;
    public Sprite Sprite;
}

[CreateAssetMenu(fileName = "ConfigMap", menuName = "Configs/ConfigMap", order = 1)]
public class ConfigMap : ScriptableObject
{
    public ConfigMapData[] Data;
}