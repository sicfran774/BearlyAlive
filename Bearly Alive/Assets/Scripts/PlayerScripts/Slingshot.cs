using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Technique
{
    // to be manipulated by designer
    public const int defaultDamage = 5;
    public const float defaultCooldown = 2f;
    public float slingSpeed = 400f;
    public float slingDuration = .7f;

    // for applying movement to actor
    private Rigidbody2D actorBody;


    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {
        actorBody = gameObject.GetComponent<Rigidbody2D>();
        base.Initialize(defaultDamage, defaultCooldown);
        
    }

    // When called, the referenced GameObject will stop moving, slide back, then shoot forward with a damaging hitbox.
    public override void Act()
    {
        if (!techsCooling)
        {
            StartCoroutine(HandleSlingShot(slingDuration));
            startCooling();
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

        // lock movement and direction
        moveLock = true;
        cursorLock = true;
        while (t < duration)
        {
            move();
            t += Time.deltaTime;
            yield return null;
        }

        // unlock movement and direction
        moveLock = false;
        cursorLock = false;

    }


}
