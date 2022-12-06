using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float projectileSpeed = 4f;

    Rigidbody2D bullet;
    Collider2D coll;
    Vector3 velocity;

    public int timeToLive = 3;

    // Start is called before the first frame update
    void Start()
    {
        bullet = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        Movement();
        Invoke("DestoryProjectile", 5f);


        // allow bouncing if Jello upgrade
        if (gameObject.tag == "UpgradeJello") {
            gameObject.AddComponent<CircleCollider2D>();
        }

    }

    // Handles the direction and velocity the bullet should travel based on 
    // Player Object. GameObject's EulerAngles are initialized in playerController.
    void Movement() {
        //Get the Screen position of the mouse
        Vector3 mouseOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mouseOnScreen - transform.position;

        //Get the angle between the points

        bullet.velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed * 10;

    }


    // Handles Collision Triggers for Bullet GameObjects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && gameObject.tag != "UpgradeRock")
        {
            DestoryProjectile();

        } 
        else if (collision.gameObject.tag == "Wall" )
        {
            if (gameObject.tag != "UpgradeJello") {
				DestoryProjectile();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col) 
    {
        timeToLive -= 1;
        if (gameObject.tag == "Enemy" || timeToLive <=0) {
            DestoryProjectile();
        }
    }



    // Destorys the projectile if the projectile has not collided with anything
    // Called by Unity's Invoke method 5 seconds after gameobject has been instantiated
    void DestoryProjectile()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
