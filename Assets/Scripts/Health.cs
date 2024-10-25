using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHp;
    private float hp;
    public Image hpGauge; // 체력바

    // 무적 시간 추가
    // 연속으로 닿아 중복으로 데미지를 입는 것을 방지하기 위한 무적 쿨타임 
    private float lastAttackTime;
    private float invincibleTime = 0.5f;

    public IHealthListener healthListener;


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        healthListener = GetComponent<IHealthListener>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 공격 받으면 체력을 계산한다
    // 1. 남은 체력이 있으면
    // 2. 데미지를 맞고난 체력을 계산한다
    // 3. Hp gauge 조절
    // 4. 죽었다면 die() 호출 

    public void Damage(float damage)
    {
        if (hp > 0 && (Time.time > lastAttackTime + invincibleTime))
        {
            hp -= damage;
            lastAttackTime = Time.time;
            if (hpGauge != null)
            {
                hpGauge.fillAmount = hp / maxHp;
            }

            Debug.Log($"Hp/MaxHp: {hp}/{maxHp}");

            if (hp <= 0)
            {
                Debug.Log("die.. sound play");
                healthListener.Die();
            }
        }
    }


    // 포션 먹을 때 
    bool Heal(int addHp)
    {
        // 현재 체력이 100%가 아니면 
        if (hp < maxHp)
        {
            // 체력 채우기
            hpGauge.fillAmount = hp / maxHp;
            return true;
        }
        else
        {
            Debug.Log("체력이 가득 찼습니다.");
            return false;
        }
    }
    private void Dead()
    {
        Debug.Log("Die..");
    }


    public interface IHealthListener
    {
        void Die();
    }
}

