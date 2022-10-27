using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

public class ForceBar : MonoBehaviour
{
    private static ForceBar _instance;
    private ForceBar() { }

    public static ForceBar Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ForceBar();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public Slider slider;
    public Text text;
    public float forceSpeed;
    public float sign;
    private bool isLocked;

    public UnityEvent OnPowerCompleted;

    void Start()
    {
        slider.value = 0;
        slider.maxValue = 1000;
        sign = 1;
        isLocked = false;

        GameManager.Instance.OnTurnChanged.AddListener(this.resetData);
        GameManager.Instance.OnPlayerShoot.AddListener(this.lockUpdate);
        GameManager.Instance.OnTurnChanged.AddListener(this.unlockUpdate);
    }

    void Update()
    {
        if (GameManager.Instance == null) return;
        if (GameManager.Instance.currentTurn == GameManager.GameTurn.None) return;
        if (isLocked) return;

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
            slider.value = -tmpForce;
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
    private void resetData()
    {
        slider.value = 0;
    }

    private void lockUpdate()
    {
        isLocked = true;
    }

    private void unlockUpdate()
    {
        isLocked = false;
    }
}
