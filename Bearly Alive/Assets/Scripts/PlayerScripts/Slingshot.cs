using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Technique
{
    // to be manipulated by designer
    public const int defaultDamage = 5;
    public const float defaultCooldown = 0.4f;
    public float slingSpeed = 500f;
    public float slingDuration = .2f;

    // reference to Weapon GameObject to access hitbox
    public GameObject SlingshotBox;

    // for applying movement to actor
    private Rigidbody2D actorBody;

    // for coordinating techniques
    private bool selfCooling;
    private float selfCooldown = 0.6f;

    // MUST BE CALLED AFTER ADD COMPONENT
    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {

        // obtain reference to weapon GameObject. <<<<< IT MUST BE THE 0th CHILD >>>>
        SlingshotBox = transform.GetChild(0).gameObject;

        // obtain reference to rigidBody for moving player
        actorBody = gameObject.GetComponent<Rigidbody2D>();

        // call overloaded initialize from Technique
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

    // sets the weapon object's tage to upgrade
    public override void SetUpgrade(string newUpgrade) {
        upgrade = newUpgrade;

        SlingshotBox.tag = newUpgrade;
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

        // activate protective&damaging hitbox. 
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
