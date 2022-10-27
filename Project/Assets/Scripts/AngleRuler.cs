using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class AngleRuler : MonoBehaviour
{
    public static AngleRuler Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AngleRuler();
            }
            return _instance;
        }
    }

    public Needle needle;
    public float maxAngle;
    public float minAngle;
    public float curAngle;
    public float rotateSpeed;

    private static AngleRuler _instance;
    private AngleRuler() { }

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        curAngle = 0;
        minAngle = -65;
        maxAngle = 115;
        rotateSpeed = 45;

        GameManager.Instance.OnTurnChanged.AddListener(this.Display);
        GameManager.Instance.OnPlayerShoot.AddListener(this.Hide);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            IncreaseAngle(Time.deltaTime * this.rotateSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            IncreaseAngle(-Time.deltaTime * this.rotateSpeed);
        }
    }

    void IncreaseAngle(float delta)
    {
        float newAngle = curAngle + delta;
        float x = transform.localScale.x;

        if (newAngle <= maxAngle && newAngle >= minAngle)
        {
            curAngle = newAngle;
            needle.IncreaseAngle(delta * x / Mathf.Abs(x));
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Display()
    {
        this.gameObject.SetActive(true);
    }
}
