using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class AngleRuler : MonoBehaviour
{
    public Needle needle;
    public int maxAngle;
    public int minAngle;
    public int curAngle;

    void Start()
    {
        curAngle = 0;
        minAngle = -65;
        maxAngle = 115;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            IncreaseAngle(1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            IncreaseAngle(-1);
        }
    }

    void IncreaseAngle(int delta)
    {
        int newAngle = curAngle + delta;
        if (newAngle <= 115 && newAngle >= minAngle)
        {
            curAngle = newAngle;
            needle.IncreaseAngle(delta);
        }
    }
}
