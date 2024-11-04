using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    //public float hp = 30;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            // 체력 회복
            collision.gameObject.GetComponent<Health>().AddHp(hp);
            GetComponent<Animator>().SetTrigger("Eat");
            Invoke("DestroyThis", 1.5f);
        }
    }
}
