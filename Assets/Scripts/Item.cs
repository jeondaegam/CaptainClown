using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //public float hp = 0;
    //public float mp = 0;

    //public float score = 0;

    //Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
        
    //}

    protected void DestroyThis()
    {
        Destroy(gameObject);
    }
}
