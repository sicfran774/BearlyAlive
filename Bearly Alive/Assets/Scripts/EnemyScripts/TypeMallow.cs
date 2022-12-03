using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeMallow : MonoBehaviour
{
    //Straightforward variables to control enemy basic attributes
    public int _MAX_HEALTH = 2;
    public float projectileSpeed = 4f;
    public HudManager hud;
    int healthRemaining;

    //Help get enemy angle and direction for attacks
    private Vector2 direction;
    private float angle;

    //A timer originally used for shots, but now used for sour and spice damage
    public float waitTime;
    private float currentTime;

    //IF enemy is poisoned or burning
    private bool isSour = false;
    private bool isSpicy = false;

    Rigidbody2D enemy;
    Collider2D coll;

    private int x;

    // Start is called before the first frame update
    void Start()
    {
        //Build enemy
        enemy = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        healthRemaining = _MAX_HEALTH;

        //Method called when delegate is invoked 
        UpgradeUI.instance.onToggleUpgradeMenu += OnUpgradeMenuToggle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //We want to shoot when the enemy "sees" the player, still every few seconds though
        //Shoot();
        if(currentTime == 0) 
        {
            if(isSour)
                tickDamage();
                x++;
            if(x == 5)
                isSour = false;
        }

        if(currentTime > 0 && currentTime % 2 == 0)
        {
            if(isSpicy)
                tickDamage();
                x++;
            if(x == 3)
                isSpicy = false;
        }
        
        //See how health is doing every frame
        if(healthRemaining <= 0) 
        {
            GameManager.instance.IncreaseScore(1);
            Destroy(gameObject);
        }

        //Timer for attacks and passive damage
        if(currentTime < waitTime)
            currentTime += 1 * Time.deltaTime;

        if(currentTime >= waitTime)
            currentTime = 0;
    }

    // Handles Enemy's objects trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit with bullet, damage the enemy
        if(collision.gameObject.tag == "Bullet")
        {
            healthRemaining--;
            if(healthRemaining <= 0)
            {
                //Increase score when enemy health is <=0
                GameManager.instance.IncreaseScore(1);
                //refresh the HUD
                //hud.refresh();

                Destroy(gameObject);

            }
        }

        //If the enemy touches something spicy or sour, will become poisoned or burning
        //If the enemy is affected by an explosion, push the enemy a certain distance

        //TODO: Change to random chance and not guarantee
        if(collision.tag == "UpgradeSour")
        {
            if(Random.Range(1, 100) <= 11)
            {
                isSour = true;
            } else {
                isSour = false;
            }
        }

        if(collision.tag == "UpgradeSpicy")
        {
            if(Random.Range(1, 100) <= 11)
            {
                isSpicy = true;
            } else {
                isSpicy = false;
            }
        }

        if(collision.tag == "UpgradeKnockback")
        {
            //TODO: Explosion!
        }
    }

    //Deal damage as per the timer given by each enemies' wait time variable
    void tickDamage()
    {
        healthRemaining--;
    }

    //Handle what happens when upgrade menu is toggled  
    void OnUpgradeMenuToggle(bool active)
    {

    }

    //Allows the enemy to face the player as it tracks them
    private Vector2 PointAtPlayer(Transform player)
    {
        Vector3 targ = player.position;
        targ.z = 0f;
        Vector3 objectPos = transform.position;

        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        angle = (Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg) - 90f;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return new Vector2(targ.x, targ.y);
    }
}