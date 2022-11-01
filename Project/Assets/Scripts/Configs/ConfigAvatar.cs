using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class ConfigAvatarData
{
    public int Id;
    public string Name;
    public GameObject Prefab;
    public Sprite Sprite;

    public int Speed;
    public int Power;
    public int Health;
    public string SpecialAbility;
}


[CreateAssetMenu(fileName = "ConfigAvatar", menuName = "Configs/ConfigAvatar", order = 1)]
public class ConfigAvatar : ScriptableObject
{
    public ConfigAvatarData[] Data;

    public ConfigAvatarData GetFromId(int avatarId) => Data.FirstOrDefault(x => x.Id == avatarId);
}
