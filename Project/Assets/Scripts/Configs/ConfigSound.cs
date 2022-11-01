using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class SoundData
{
    public int Id;
    public string Name;
    public AudioSource Prefab;
}


[CreateAssetMenu(fileName = "ConfigSound", menuName = "Configs/ConfigSound", order = 2)]
public class ConfigSound : ScriptableObject
{
    public SoundData[] Data;

    public SoundData GetFromId(int soundID) => Data.FirstOrDefault(x => x.Id == soundID);
}
