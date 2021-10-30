using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public Slider healthBar;
    public UnityEvent OnDestroyed;

    float health;

    void Start()
    {
        SetDefaults();
    }

    private void SetDefaults()
    {
        health = maxHealth;
        healthBar.value = health / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (health > damageAmount)
        {
            health = health - damageAmount;
            healthBar.value = health / maxHealth;
        }
        else
        {
            //killed
            if (OnDestroyed != null)
                OnDestroyed.Invoke();
        }
    }

    public void HideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}
