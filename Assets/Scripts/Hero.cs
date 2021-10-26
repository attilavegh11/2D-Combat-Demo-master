using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float fightDistance = 2f;
    public float health = 100f;

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
        RaycastHit2D hit = Physics2D.Raycast(eyesightPoint.transform.position, -Vector2.right);
        Debug.DrawRay(eyesightPoint.transform.position, -transform.right * 100, Color.green);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Enemy")
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
}
