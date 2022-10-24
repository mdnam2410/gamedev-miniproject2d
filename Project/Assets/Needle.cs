using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    public void IncreaseAngle(float delta)
    {
        float x = transform.localScale.x;
        float sign = x / Mathf.Abs(x);
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - delta);
    }
}
