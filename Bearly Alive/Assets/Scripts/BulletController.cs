using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float projectileSpeed = 10f;


    Rigidbody2D bullet;
    Collider2D coll;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        bullet = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        print(transform.rotation);
        Movement();
        Invoke("DestoryProjectile", 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Handles the direction and velocity the bullet should travel based on 
    // Player Object. GameObject's EulerAngles are initialized in playerController.
    void Movement() {
        float xSpeed = 0f;
        float ySpeed = 0f;
        Vector3 direction = transform.rotation.eulerAngles;
        print(direction);
        if(direction.z == 90f)
        {
            print("Left");
            xSpeed = projectileSpeed * -1;
        }
        else if(direction.z == 270f)
        {
            print("Right");
            xSpeed = projectileSpeed;
        }
        else if(direction.z == 180f)
        {
            ySpeed = projectileSpeed * -1;
        }
        else if(direction.z == 0f)
        {
            ySpeed = projectileSpeed;
        }

        bullet.velocity = new Vector3(xSpeed, ySpeed, 0f);

    }


    // Handles Collision Triggers for Bullet GameObjects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
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
