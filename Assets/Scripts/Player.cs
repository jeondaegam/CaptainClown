using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 6f;

    // 방향 
    private float velocityX;
    // 이전 방향 
    private float prevVx;

    //
    private Animator animator;

    // 지면 인식 
    private bool isGrounded;
    public Collider2D bottomCollider;
    public CompositeCollider2D terrainCollider;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 방향키 입력
        velocityX = Input.GetAxisRaw("Horizontal");
        UpdateCharacterDirection();

        Debug.Log(velocityX);
        Debug.Log("isGround:" + isGrounded);


        // 지면 인식 및 애니메이션 실행
        UpdateGroundedStateAndAnimation();


        // 점프
        if (Input.GetButtonDown("Jump"))
        {
            // is Grounded로 2단점프 제한 (아이템 사용으로 ㅈ점프 부스터 )
            GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;
        }

        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("SwordAttack1");
        }

        prevVx = velocityX;


    }


    // 좌우 이동 
    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * velocityX * speed * Time.fixedDeltaTime);
    }

    private void UpdateCharacterDirection()
    {
        // 이동 방향에 따른 캐릭터 방향 변경 
        if (velocityX > 0)
        {
            // 우측 이동 
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (velocityX < 0)
        {
            // 좌측 이동
            GetComponent<SpriteRenderer>().flipX = true;
        }
        // 0 일 때는 이전 방향 유지 
    }


    private void UpdateGroundedStateAndAnimation()
    {
        if (bottomCollider.IsTouching(terrainCollider))
        {
            // 1. 이전에 공중에 있었다면 (지면에 착지한 상태)
            if (!isGrounded)
            {
                // 이동 속도가 0이면 
                if (velocityX == 0)
                {
                    animator.SetTrigger("Idle");
                }
                else
                {
                    animator.SetTrigger("Run");
                }
            }
            else // 2. 이전에 땅에 있었다면 
            {
                if (prevVx != velocityX)
                {
                    if (velocityX == 0)
                    {
                        animator.SetTrigger("Idle");
                    }
                    else
                    {
                        animator.SetTrigger("Run");
                    }
                }
            }
            isGrounded = true;
        }
        else
        {
            // 지금 terrain과 닿지 않는데 이전에 땅에 있었다 ? => 지금은 점프중
            if (isGrounded)
            {
                animator.SetTrigger("Jump");
            }
            isGrounded = false;
        }
    }
}



