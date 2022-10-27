using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public HealthBar healthBar;
    public int health = 0;

    private void Update()
    {
        if (this.health != 0)
        {
            this.healthBar.SetHealth(this.health);
        }
    }

}
