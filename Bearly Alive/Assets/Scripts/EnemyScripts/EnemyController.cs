using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int _MAX_HEALTH = 2;
    public HudManager hud;
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
        if(collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Sword")
        {
            if(collision.gameObject.tag == "Sword")
            {
                healthRemaining -= 2;
            }
            else
            {
                healthRemaining--;
            }
            
            if(healthRemaining <= 0)
            {
                //Increase score when enemy health is <=0
                GameManager.instance.IncreaseScore(1);
                //refresh the HUD
                //hud.refresh();

                Destroy(gameObject);

            }
        }

    }



}
