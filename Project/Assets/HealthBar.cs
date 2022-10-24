using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text text;

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        updateText();
    }

    public void setHealth(int health)
    {
        slider.value = health;

        updateText();
    }

    private void updateText()
    {
        text.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
    } 
}
