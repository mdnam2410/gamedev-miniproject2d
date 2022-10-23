using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
    public void IncreaseAngle(int delta)
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - delta);
    }
}
