using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int _MAX_HEALTH = 2;

    int healthRemaining;

    Rigidbody2D enemy;
    Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        healthRemaining = _MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Handles Enemy's objects trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            healthRemaining--;
            if(healthRemaining <= 0)
            {
                Destroy(gameObject);
            }
        }
    }



}
