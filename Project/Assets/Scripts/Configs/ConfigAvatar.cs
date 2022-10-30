using System;
using UnityEngine;

[Serializable]
public class ConfigAvatarData
{
    public int Id;
    public string Name;
    public GameObject Prefab;
}


[CreateAssetMenu(fileName = "ConfigAvatar", menuName = "Configs/ConfigAvatar", order = 1)]
public class ConfigAvatar : ScriptableObject
{
    public ConfigAvatarData[] Data;
}
