using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public Slider healthBar;

    void Start()
    {
        SetDefaults();
    }

    private void SetDefaults()
    {
        healthBar.value = health / 100;
    }

    public void TakeDamage(float damageAmount)
    {
        if (health > damageAmount)
        {
            health -= damageAmount;
            healthBar.value = health / 100;
        }
        else
        {
            //killed
            Destroy(gameObject);
        }
    }
}
