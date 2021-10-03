using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fightDistance = 2f;
    public float health = 100f;
    public float damageAmount = 10f;
    public GameObject eyesightPoint;
    public TextMeshProUGUI healthText;

    void Start()
    {
        SetDefaults();
    }

    private void SetDefaults()
    {
        healthText.text = "HP: " + health;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyesightPoint.transform.position, Vector2.right);
        Debug.DrawRay(eyesightPoint.transform.position, -transform.right * 100, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Hero")
            {
                if (!WithinRadius(hit.collider.gameObject))
                {
                    float steps = moveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, hit.collider.gameObject.transform.position, steps);
                }
            }
        }
    }

    bool WithinRadius(GameObject enemy)
    {
        if (Vector2.Distance(transform.position, enemy.transform.position) < fightDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (health > 0)
        {
            health = health - damageAmount;
            healthText.text = "HP: " + health;
        }
        else
        {
            Destroy(gameObject);

            //spawn VFX
        }
    }
}