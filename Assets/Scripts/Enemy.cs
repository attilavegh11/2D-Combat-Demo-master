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
    public Fist fist;

    [Header("Animation")]
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, attack, death;
    public string currentState;
    public float speed;
    public float movement;
    private Rigidbody2D rigidbody;
    public string currentAnimation;

    private bool isAttacking;
    internal bool dead;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = "Walking";
        SetCharacterState(currentState);
    }

    void Update()
    {
        if (!dead)
        {
            Move();
        }
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
        }
        else if (state.Equals("Attack"))
        {
            SetAnimation(attack, true, 1f);
        }
        else if (state.Equals("Death"))
        {
            SetAnimation(death, false, 1f);
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
                //assign enemy to fist if there's no enemy in target
                if (fist.enemyInfront == null)
                {
                    fist.enemyInfront = hit.collider.gameObject;
                }
                else
                {
                    //if current target is the base but there's another enemy in front, reassign
                    if (fist.enemyInfront.tag == "Hero Base" && hit.collider.gameObject.tag == "Hero")
                    {
                        fist.enemyInfront = hit.collider.gameObject;
                    }
                }

                if (!WithinRadius(fist.enemyInfront))
                {
                    //move towards
                    float steps = moveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, fist.enemyInfront.transform.position, steps);

                    StartWalking();
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
                StartWalking();
            }
        }
    }

    void StartWalking()
    {
        currentState = "Walking";
        SetCharacterState(currentState);
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

    public void OnDeath()
    {
        Debug.Log("Enemy OnDeath");
        if (dead)
            return;
        dead = true;

        //disable all colliders
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Health>().HideHealthBar();
        fist.enemyInfront.GetComponent<PlayerMovement>().fist.enemyInfront = null;

        currentState = "Death";
        SetCharacterState(currentState);
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        yield return null;
    }
}