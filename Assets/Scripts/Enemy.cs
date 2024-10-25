using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float damage = 1f;
    public float defaultDamage = 1f;

    private float speed = 0.7f;
    private Vector2 velocityX;
    private Vector2 prevX;

    public Transform player;

    // 절벽, 장애물 감지
    public CompositeCollider2D terrainCollider;
    public Collider2D bottomCollider;
    public Collider2D frontCollider;

    private enum State
    {
        Idle,
        Walk,
        Attack,
        Dying
    }

    private State state;
    private float timeForNextState = 2f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // 좌우 이동
        velocityX = Vector2.left * speed;
        prevX = velocityX;

        // init
        animator = GetComponent<Animator>();

        int randomNum = Random.Range(0, 2);

        //(randomNum == 0 ? (Action)StartIdle : StartWalk)();

        if (randomNum == 0)
        {
            StartIdle();
        }
        else
        {
            StartWalk();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("velocity X:" + velocityX);


        switch (state)
        {
            case State.Idle:

                timeForNextState -= Time.deltaTime;

                if (timeForNextState < 0)
                {
                    // 휴식이 끝나면 걷기 시작 
                    StartWalk();
                }
                break;

            case State.Walk:

                timeForNextState -= Time.deltaTime;
                DetectAndFlipDirection();

                if (timeForNextState < 0)
                {
                    // 걷기가 끝나면 휴식 시작 
                    StartIdle();
                }
                break;

        }

    }


    private void DetectAndFlipDirection()
    {
        // 절벽 or 장애물을 만나면
        if (frontCollider.IsTouching(terrainCollider)
            || !bottomCollider.IsTouching(terrainCollider))
        {
            // 방향 전환 
            velocityX *= -1;
            transform.localScale = new Vector2(-transform.localScale.x, 1);
        }
    }

    private void StartWalk()
    {
        state = State.Walk;
        velocityX = prevX;
        animator.SetTrigger("Walk");
        timeForNextState = Random.Range(3f, 6f);
        Debug.Log(timeForNextState);
    }

    private void StartIdle()
    {
        state = State.Idle;
        prevX = velocityX;
        velocityX = Vector2.zero;
        animator.SetTrigger("Idle");
        timeForNextState = Random.Range(1f, 2.5f);
        Debug.Log(timeForNextState);
    }


    // 이동
    private void FixedUpdate()
    {
        transform.Translate(velocityX * Time.fixedDeltaTime);
    }

    // 플레이어가 몸에 닿으면 잔상을 입힌다
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().Damage(defaultDamage);
        }
    }
}
