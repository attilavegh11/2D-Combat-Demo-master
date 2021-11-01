using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    [Tooltip("GameObjects with this tag will be attacked by this character")]
    public float attackPower = 10;
    public Health characterHealth;
    internal GameObject enemyInfront;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == enemyInfront)
        {
            //check if this attacker is alive
            if (!characterHealth.dead)
            {
                other.gameObject.GetComponent<Health>().TakeDamage(attackPower);
            }
        }
    }
}
