using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConfigTankData
{
    public string Name;
    public GameObject Prefab;
}

[CreateAssetMenu(fileName = "ConfigTank", menuName = "Configs/ConfigTank", order = 1)]
public class ConfigTank : ScriptableObject
{
    public ConfigTankData[] Data;
}
