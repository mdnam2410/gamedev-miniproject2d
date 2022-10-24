using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForceBar : MonoBehaviour
{
    public Slider slider;
    public Text text;
    public float forceSpeed;

    void Start()
    {
        slider.value = 0;
        slider.maxValue = 1000;
        forceSpeed = 30;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            UpdateForce(Time.deltaTime * this.forceSpeed);
        }
    }

    public void UpdateForce(float delta)
    {
        slider.value += delta;

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
