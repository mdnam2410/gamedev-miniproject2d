using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ForceBar : MonoBehaviour
{
    public static ForceBar instance;
    public Slider slider;
    public Text text;
    public float forceSpeed;
    public float sign;

    public UnityEvent OnPowerCompleted;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        slider.value = 0;
        slider.maxValue = 1000;
        sign = 1;
    }

    void Update()
    {
        if (GameManager.instance == null) return;
        if (GameManager.instance.currentTurn == GameManager.GameTurn.None) return;

        if (Input.GetKey(KeyCode.Space))
        {
            UpdateForce(Time.deltaTime * this.forceSpeed);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            this.OnPowerCompleted.Invoke();
        }
    }

    public void UpdateForce(float delta)
    {
        float tmpForce = slider.value + delta * sign;

        if (tmpForce >= slider.maxValue)
        {
            sign = -1;
            slider.value = slider.maxValue * 2 - tmpForce;
        }
        else if (tmpForce <= 0)
        {
            sign = 1;
            slider.value = - tmpForce;
        }
        else
        {
            slider.value = tmpForce;
        }
        //updateText();
    }

    public float getMaxForce()
    {
        return slider.maxValue;
    }

    public float getForce()
    {
        return slider.value;
    }

    private void updateText()
    {
        text.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
    }
}
