using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : Technique
{

    private GameObject boomerang;

    // The amount of time for slash action to finish
    public float duration = 0.7f;

    public float tossSpeed = 5f;

    public float standDuration = 0.7f;

    public float returnSpeed = 5f;

    public float rotationSpeed = 600f;

    bool returning = false;

    Rigidbody2D boomerangRigidBody;
    Collider2D coll;

    // to be manipulated by designer
    public const int defaultDamage = 10;
    // Time to wait until player is able to use technique again
    public const float defaultCooldown = 1f;


    // Makes the sword hidden when it is first initializes when the game starts
    // ALWAYS call after add component
    public override void Initialize(int damage = defaultDamage, float cooldown = defaultCooldown)
    {
        base.Initialize(defaultDamage, defaultCooldown);
    }

    public override void Act()
    {
        if (!techsCooling)
        {
            StartCoroutine(HandleBoomerangToss(duration));
            techsCooling = true;
        }
    }


    public override void SetUpgrade(string newUpgrade)
    {
        // TODO
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Boomerang")
        {
            if (returning)
            {
                print("Player Caught Boomerang");
                returning = false;
            }
        }
    }



    // Coroutine that handles the boomerang technique.
    IEnumerator HandleBoomerangToss(float duration)
    {
        // Create Boomerang Game Object
        boomerang = Instantiate(GameManager.instance.Boomerang, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0f, 0f, 0f));
        // Get RigidBody and Collider components
        boomerangRigidBody = boomerang.GetComponent<Rigidbody2D>();
        coll = boomerang.GetComponent<Collider2D>();
        // Make Boomerang Visible
        boomerang.SetActive(true);


        float t = 0.0f;

        //Get the Screen position of the mouse
        Vector3 mouseOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = mouseOnScreen - transform.position;

        // Set Speed of Boomerang to travel in the direction of the mouse cursor
        boomerangRigidBody.velocity = new Vector2(direction.x, direction.y).normalized * tossSpeed * 10;

        // Boomerang's Toss and Travel Duration
        while (t < duration)
        {
            //distance (in angles) to rotate on each frame. distance = speed * time
            float angle = rotationSpeed * Time.deltaTime;
            
            boomerang.transform.Rotate(Vector3.forward * angle, Space.World);
            t += Time.deltaTime;

            yield return null;
        }

        // Set Boomerang's velocity to 0
        t = 0;
        boomerangRigidBody.velocity = Vector2.zero;

        // Duration for how long the boomerang should wait before returning to player
        while (t < standDuration)
        {
            //distance (in angles) to rotate on each frame. distance = speed * time
            float angle = rotationSpeed * Time.deltaTime;

            boomerang.transform.Rotate(Vector3.forward * angle, Space.World);
            t += Time.deltaTime;
            yield return null;
        }

        //  Start Boomerang to follow Player to Catch for next use
        returning = true;
        while (returning)
        {
            float angle = rotationSpeed * Time.deltaTime;

            boomerang.transform.Rotate(Vector3.forward * angle, Space.World);

            Vector3 playerPosition = transform.position;
            Vector3 boomerangPosition = boomerang.transform.position;
            Vector3 dir = boomerangPosition - transform.position;

            boomerangRigidBody.velocity = new Vector2(dir.x, dir.y).normalized * returnSpeed * 10 * -1;

            yield return null;
        }

        // Boomerang has been caught
        Destroy(boomerang);
        // Reset cooldown
        techsCooling = false;


    }

}