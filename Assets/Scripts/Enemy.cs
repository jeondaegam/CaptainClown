using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = .7f;
    private Vector2 velocityX;

    public CompositeCollider2D terrainCollider;
    public Collider2D bottomCollider;
    public Collider2D frontCollider;



    // Start is called before the first frame update
    void Start()
    {
        // 좌우 이동
        GetComponent<Animator>().SetTrigger("Run");
        //speed값을 start문에서 한번만 초기화 하기 때문에
        // 중간에 인스펙터로 수정하는건 의미없음 
        velocityX = Vector2.left * speed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("speed : " + speed);
        // 절벽이나 장애물을 마주치면
        if (frontCollider.IsTouching(terrainCollider)
            || !bottomCollider.IsTouching(terrainCollider))
        {
            // 방향 바꾸기
            velocityX *= -1;
            transform.localScale = new Vector2(-transform.localScale.x, 1);
        }
    }




    private void FixedUpdate()
    {
        transform.Translate(velocityX * Time.fixedDeltaTime);
    }
}
