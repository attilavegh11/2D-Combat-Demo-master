using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public enum Type { Hero, Enemy }
    public Type type;
    public float maxHealth = 100f;
    public Slider healthBar;
    public UnityEvent OnDestroyed;
    public float health;
    internal bool dead;

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
            dead = true;

            //killed
            if (OnDestroyed != null)
            {
                /*if(type == Type.Enemy)
                {
                    GetComponent<Enemy>().OnDeath();
                }
                else
                {
                    GetComponent<PlayerMovement>().OnDeath();
                }*/

                OnDestroyed.Invoke();
            }
        }
    }

    public void HideHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }
}
