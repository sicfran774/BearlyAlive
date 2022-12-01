using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Technique
{
    // to be manipulated by designer
    public const int defaultDamage = 5;
    public const float defaultCooldown = .9f;
    public float slingSpeed = 300f;
    public float slingDuration = .7f;

    public GameObject SlingshotBox; 

    // for applying movement to actor
    private Rigidbody2D actorBody;

    // for coordinating techniques
    private bool selfCooling;
    public float selfCooldown = 1.1f;


    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {
        actorBody = gameObject.GetComponent<Rigidbody2D>();
        base.Initialize(defaultDamage, defaultCooldown);

        // get SlingshotBox from gameObject
        SlingshotBox = transform.GetChild(0).gameObject;
        
    }

    // When called, the referenced GameObject will stop moving, slide back, then shoot forward with a damaging hitbox.
    public override void Act()
    {
        if (!techsCooling && !selfCooling)
        {
            // set coordination booleans
            techsCooling = true;
            selfCooling = true;

            //perform the movement and enable/diable hitbox
            StartCoroutine(HandleSlingShot(slingDuration));
        }

    }

    public override void SetUpgrade(string upgrade) {
        base.upgrade = upgrade;// placeholder implementation.
    }
    
    private void move() {

        Vector2 currPosition = transform.position;
        Vector2 displacement = transform.up * Time.deltaTime * slingSpeed;
        currPosition += displacement;
        actorBody.MovePosition(currPosition);
    
    }

    IEnumerator HandleSlingShot(float duration)
    {
        float t = 0.0f;

        // enable damaging/protecting hitbox holder
        SlingshotBox.SetActive(true);

        // lock movement and direction
        moveLock = true;
        cursorLock = true;

        // pause
        while(t < cooldown - duration) {
            t+= Time.deltaTime;
            
            yield return null;
        }
        // sling
        while (t < cooldown)
        {
            move();
            t += Time.deltaTime;
            yield return null;
        }

        // disable damaging/protecting hitbox holder
        SlingshotBox.SetActive(false);

        // unlock movement and direction
        moveLock = false;
        cursorLock = false;
        techsCooling = false;

        // cool self
        print(t);
        while (t < selfCooldown)
        {
            t += Time.deltaTime;
            
            yield return null;
        }
        print(t);
        print("self cooled");
        selfCooling = false;
    }


}
