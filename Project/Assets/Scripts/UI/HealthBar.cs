using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text text;
    public TMPro.TMP_Text playerName;

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

    public void SetPlayerName(string name) => playerName.text = name;

    private void UpdateText()
    {
        text.text = slider.value.ToString() + "/" + slider.maxValue.ToString();
    } 
}
