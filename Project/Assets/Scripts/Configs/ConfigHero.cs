using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ConfigHeroData
{
    public string Name;
    public string SpritePath;
    public string PrefabPath;
}

[CreateAssetMenu(fileName = "ConfigHero", menuName = "Configs/ConfigHero", order = 1)]
public class ConfigHero : ScriptableObject
{
    public ConfigHeroData[] Data;
}
