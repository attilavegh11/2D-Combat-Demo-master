using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Type { Hero, Enemy }
    public Type type;
    public float movementSpeed = 5;
    public float attackPower = 10; //damage dealt by the bullet
    public Health characterHealth;
    internal GameObject enemyInfront;

    void Start()
    {

    }

    void Update()
    {
        //move forward
        GetComponent<Rigidbody2D>().AddForce(Vector2.right * movementSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if this attacker is alive
        if (other.gameObject.GetComponent<Health>() != null)
        {
            other.gameObject.GetComponent<Health>().TakeDamage(attackPower);
        }
    }
}
