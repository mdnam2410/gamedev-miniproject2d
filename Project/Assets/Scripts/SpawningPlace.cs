using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPlace : MonoBehaviour
{
    public List<Transform> listPlace1;
    public List<Transform> listPlace2;

    public static SpawningPlace Instance;

    private void Awake()
    {
        Instance = this;
    }
}
