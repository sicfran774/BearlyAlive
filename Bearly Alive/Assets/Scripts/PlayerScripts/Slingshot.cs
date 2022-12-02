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

    // to access hitbox
    public GameObject SlingshotBox;

    // for applying movement to actor
    private Rigidbody2D actorBody;

    // for coordinating techniques
    private bool selfCooling;
    private float selfCooldown = 1.1f;

    // MUST BE CALLED AFTER ADD COMPONENT
    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {
        actorBody = gameObject.GetComponent<Rigidbody2D>();
        base.Initialize(defaultDamage, defaultCooldown);
        
    }

    // When called, the referenced GameObject will stop moving, slide back, then shoot forward with a damaging hitbox.
    public override void Act()
    {
        if (!techsCooling && !selfCooling)
        {
            StartCoroutine(HandleSlingShot(slingDuration));
        }

    }

    public override void SetUpgrade(string upgrade) {
        base.upgrade = upgrade;
        SlingshotBox.tag = upgrade;
    }
    
    private void move() {

        Vector2 currPosition = transform.position;
        Vector2 displacement = transform.up * Time.deltaTime * slingSpeed;
        currPosition += displacement;
        actorBody.MovePosition(currPosition);
    
    }

    IEnumerator HandleSlingShot(float duration)
    {
        

        // set inter-technique coordination variables
        techsCooling = true;
        selfCooling = true;

        float t = 0.0f;

        // activate protective&damaging hitbox. IT MUST BE THE 0th CHILD
        SlingshotBox = transform.GetChild(0).gameObject;
        SlingshotBox.SetActive(true);

        // lock movement and direction
        moveLock = true;
        cursorLock = true;
        while(t < cooldown - duration) {
            t+= Time.deltaTime;
            
            yield return null;
        }
        while (t < cooldown)
        {
            move();
            t += Time.deltaTime;
            yield return null;
        }

        // unlock movement and direction
        moveLock = false;
        cursorLock = false;
        techsCooling = false;

        // deactivate protective&damaging hitbox. IT MUST BE THE 0th CHILD
        SlingshotBox = transform.GetChild(0).gameObject;
        SlingshotBox.SetActive(false);

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
