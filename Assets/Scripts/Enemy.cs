using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Properties")]
    public float moveSpeed = 2f;
    public float fightDistance = 2f;
    public float eyesightDistance = 20f;
    public GameObject eyesightPoint;
    public LayerMask layerMask;

    [Header("Animation")]
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, attack;
    public string currentState;
    public float speed;
    public float movement;
    private Rigidbody2D rigidbody;
    public string currentAnimation;

    private bool isAttacking;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = "Walking";
        SetCharacterState(currentState);
    }

    void Update()
    {
        Move();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    public void SetCharacterState(string state)
    {
        if (state.Equals("Walking"))
        {
            SetAnimation(walking, true, 1f);
            //defaultCollider.enabled = true;
            //combatCollider.enabled = false;
        }
        else if (state.Equals("Attack"))
        {
            SetAnimation(attack, true, 1f);
            //defaultCollider.enabled = false;
            //combatCollider.enabled = true;
        }
    }

    public void Move()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyesightPoint.transform.position, -Vector2.right, eyesightDistance, layerMask);
        Debug.DrawRay(eyesightPoint.transform.position, transform.right * eyesightDistance, Color.green);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Hero" || hit.collider.gameObject.tag == "Hero Base")
            {
                if (!WithinRadius(hit.collider.gameObject))
                {
                    if (!isAttacking)
                    {
                        //move towards
                        float steps = moveSpeed * Time.deltaTime;
                        transform.position = Vector2.MoveTowards(transform.position, hit.collider.gameObject.transform.position, steps);
                    }
                }
                else
                {
                    //start attacking
                    SetCharacterState("Attack");
                    isAttacking = true;
                }
            }
            else
            {
                isAttacking = false;
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

    //temporary. move this method to a new class called Fist
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Hero")
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(10);
        }
    }
}