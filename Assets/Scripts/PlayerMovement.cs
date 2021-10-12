using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerMovement : MonoBehaviour
{
    [Header("Properties")]
    public float moveSpeed = 2f;
    public float fightDistance = 2f;
    public float eyesightDistance = 20f;
    public GameObject eyesightPoint;
    public LayerMask layerMask;

    /*[Header("Collission")]
    public Collider2D defaultCollider;
    public Collider2D combatCollider;*/

    [Header("Animation")]
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, walking, attack;
    public string currentState;
    public float speed;
    public float movement;
    private Rigidbody2D rigidbody;
    public string currentAnimation;    
    
    
    // Start is called before the first frame update
    void Start()
    {
        //defaultCollider.enabled = true;
        //combatCollider.enabled = false;
        
        rigidbody = GetComponent<Rigidbody2D>();
        currentState = "Walking";
        SetCharacterState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        RaycastHit2D hit = Physics2D.Raycast(eyesightPoint.transform.position, Vector2.right, eyesightDistance, layerMask);
        Debug.DrawRay(eyesightPoint.transform.position, transform.right * eyesightDistance, Color.green);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Enemy" || hit.collider.gameObject.tag == "Enemy Base")
            {
                if (!WithinRadius(hit.collider.gameObject))
                {
                    //move towards
                    float steps = moveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, hit.collider.gameObject.transform.position, steps);
                }
                else
                {
                    //start attacking
                    SetCharacterState("Attack");
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
