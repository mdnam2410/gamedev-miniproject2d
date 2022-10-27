using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        UpdateText();
    }

    public void SetHealth(int health)
    {
        slider.value = health;

        UpdateText();
    }

    private void UpdateText()
    {
        text.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
    } 
}
