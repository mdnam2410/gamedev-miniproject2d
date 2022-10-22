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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {

        }
    }

    void IncreaseAngle(int delta)
    {
        curAngle += delta;
        needle.transform.eulerAngles = new Vector3(0, 0, )
    }
}
