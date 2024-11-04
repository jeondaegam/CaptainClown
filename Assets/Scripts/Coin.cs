using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : Item
{

    public int score;
    public TextMeshProUGUI scoreLabel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // score up
            scoreLabel.text = (int.Parse(scoreLabel.text) + score).ToString();
            GetComponent<Animator>().SetTrigger("Eat");
            Invoke("DestroyThis", 1.5f);
            
        }
    }
}
