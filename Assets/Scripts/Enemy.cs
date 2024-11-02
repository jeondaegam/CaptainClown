using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [Header("기본 스텟")]
    public float attackDamage = 1f;
    public float defaultDamage = 1f;

    private float speed = 0.7f;
    private Vector2 velocityX;
    private Vector2 prevX;


    [Header("절벽, 장애물 감지")]
    public CompositeCollider2D terrainCollider;
    public Collider2D bottomCollider;
    public Collider2D frontCollider;

    // 플레이어 공격
    [Header("플레이어 감지, 공격")]
    public Collider2D attackCollider;
    public Collider2D playerCollider;
    public Transform player;


    private enum State
    {
        Idle,
        Walk,
        Chase,
        Attack,
        Dying
    }

    private State state;
    private float timeForNextState = 2f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // 이동 방향 벡터 초기화 
        velocityX = Vector2.left * speed;
        prevX = velocityX;

        // init
        animator = GetComponent<Animator>();

        int randomNum = Random.Range(0, 2);
        (randomNum == 0 ? (Action)StartIdle : StartWalk)();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("velocity X:" + velocityX);
         
        float distance = (player.position - transform.position).magnitude;
        //Debug.Log("distance : " + distance.ToString("##.#"));

        switch (state)
        {
            case State.Idle:

                timeForNextState -= Time.deltaTime;

                if (distance < 1.0f)
                {
                    // 시야에 들어오면 추적
                    // 느낌표 애니메이션

                    //StartChase();
                    Attack();
                }

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

            //case State.Chase:

            //    //FaceTarget();
            //    //DetectAndFlipDirection();

            //    // 시야 밖으로 나가면 휴식
            //    //if (playerDistance > 4.0f)
            //    //{
            //    //    Debug.Log("범위를 벗어났습니다 . ");
            //    //    StartIdle();
            //    //}
            //    //// 가까이 근접하면 공격
            //    //else if (playerDistance < 1.0f)
            //    //{
            //    //    //
            //    //    Debug.Log("근접해서 공격합니다 ");
            //    //    Attack();
            //    //}

            //    else
            //    {
            //        Vector2 direction = (player.position - transform.position).normalized;
            //        direction.y = 0;
            //        //prevX = velocityX;
            //        velocityX = direction * speed;
            //        //Debug.Log("direction " + direction);
            //    }

            //    break;

            case State.Attack:
                timeForNextState -= Time.deltaTime;

                if (attackCollider.IsTouching(playerCollider))
                {
                    Debug.Log("공격 해요 ! ");
                    playerCollider.GetComponent<Health>().Damage(attackDamage);
                }

                if (timeForNextState < 0)
                {
                    // 공격중 쿨타임은 일정했으면 좋겠다 1.0f
                    // TODO - 공격중에 낭떠러지를 만나면 떨어져버림 (절벽에서 이동을 멈췄으면 좋겠다 )
                    StartIdle();
                }
                break;
        }

    }

    //private void StartChase()
    //{
    //    Debug.Log("플레이어 추적 시작 ! ");
    //    state = State.Chase;
    //    animator.SetTrigger("Walk");
    //    Debug.Log("Chase State : " + velocityX);
    //}

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
        //if (state == State.Chase)
        //{
        //    state = State.Walk;
        //}
    }

    //private void FaceTarget()
    //{
    //    //velocityX *= -1;
    //    //transform.localScale = new Vector2(-transform.localScale.x, 1);
    //    // 플레이어가 왼쪽에 있을 때 
    //    if (player.position.x - transform.position.x < 0)
    //    {
    //        //
    //        Debug.Log("왼쪽 따라가용 ");
    //        transform.localScale = new Vector2(-1, 1);
    //        Debug.Log("local scale 왼쪽 따라갈때 " + transform.localScale);
    //    } else
    //    {
    //        // 오른쪽에 있을 때
    //        Debug.Log("d오른쪽 따라가용 ");
    //        transform.localScale = new Vector2(1, 1);
    //        Debug.Log("local scale 오른쪽  따라갈때 " + transform.localScale);
    //    }
    //}


    private void StartWalk()
    {
        //
        state = State.Walk;
        // 이전에 걷던 방향으로 걸어간다 
        velocityX = prevX;
        animator.SetTrigger("Walk");
        timeForNextState = Random.Range(3f, 6f);
        Debug.Log("걷기 시작 :" + velocityX);
    }

    private void StartIdle()
    {
        //
        //if (state != State.Chase)
        //{
        prevX = velocityX;
        //}
        state = State.Idle;
        // 이전 걷던 방향을 기억
        velocityX = Vector2.zero;
        animator.SetTrigger("Idle");
        timeForNextState = Random.Range(1f, 2.5f);
        Debug.Log("휴식 시작 : " + velocityX) ;

        // set collider
        attackCollider.enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    private void Attack()
    {
        velocityX = prevX;
        state = State.Attack;
        animator.SetTrigger("Attack");
        timeForNextState = 2.0f;
        // set collider
        attackCollider.enabled = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
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
            Debug.Log("잔상");
            collision.gameObject.GetComponent<Health>().Damage(defaultDamage);
        }
    }


    /*
     
     1. Attack Collider를 만들고 [V]
    2.  공격하는 순간에만 On, 기본 콜라이더는 Off [V]
    3. 공격이 끝나면 콜라이더 스위칭 [V]

    공격 프로세스
    1. 시야에 들어오면
    2. 타깃 위치를 포착
    3. 그곳으로 드리프트 (공격)
    4. 어택 콜라이더에 플레이어가 닿았으면 데미지 [V]
    4. 휴식 
     

    현재 프로세스
    1. 플레이어와 닿으면
    2. 프레이어에게 다가가면서 + (공격)
    3. 몬스터 콜라이더에 플레이어가 닿으면 데미지


     */
}
