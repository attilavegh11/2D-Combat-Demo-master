using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerMovement : MonoBehaviour
{
    public enum CombatType { Melee, Range }

    [Header("Properties")]
    public float moveSpeed = 2f;
    public float fightDistance = 2f;
    public float eyesightDistance = 20f;
    public float attackFrequency = 1f;

    public CombatType combatType;
    public GameObject eyesightPoint;
    public LayerMask layerMask;
    public Fist fist;
    public Canon canon;

    /*[Header("Collission")]
    public Collider2D defaultCollider;
    public Collider2D combatCollider;*/

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

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = "Walking";
        SetCharacterState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (combatType == CombatType.Melee)
            {
                //melee
                HandleMeleeCombat();
            }
            else
            {
                //range
                HandleRangeCombat();
            }
        }
    }

    //sets character animation
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    //checks character state and sets the animation accordingly
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

    public void HandleMeleeCombat()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyesightPoint.transform.position, Vector2.right, eyesightDistance, layerMask);
        Debug.DrawRay(eyesightPoint.transform.position, transform.right * eyesightDistance, Color.green);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Enemy Base")
            {
                //assign enemy to fist if there's no enemy in target
                if (fist.enemyInfront == null)
                {
                    fist.enemyInfront = hit.collider.gameObject;
                }
                else
                {
                    //if current target is the base but there's another enemy in front, reassign
                    if (fist.enemyInfront.tag == "Enemy Base" && hit.collider.gameObject.tag == "Enemy")
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

    public void HandleRangeCombat()
    {
        RaycastHit2D hit = Physics2D.Raycast(eyesightPoint.transform.position, Vector2.right, eyesightDistance, layerMask);
        Debug.DrawRay(eyesightPoint.transform.position, transform.right * eyesightDistance, Color.green);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Enemy Base")
            {
                //assign enemy to fist if there's no enemy in target
                if (canon.enemyInfront == null)
                {
                    canon.enemyInfront = hit.collider.gameObject;
                }
                else
                {
                    //if current target is the base but there's another enemy in front, reassign
                    if (canon.enemyInfront.tag == "Enemy Base" && hit.collider.gameObject.tag == "Enemy")
                    {
                        canon.enemyInfront = hit.collider.gameObject;
                    }
                }

                if (!WithinRadius(canon.enemyInfront))
                {
                    //move towards
                    float steps = moveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, canon.enemyInfront.transform.position, steps);

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
        Debug.Log("Hero OnDeath");
        if (dead)
            return;
        dead = true;

        //disable all colliders
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<Health>().HideHealthBar();
        fist.enemyInfront.GetComponent<Enemy>().fist.enemyInfront = null;

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
