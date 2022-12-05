using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : Technique
{
    // to be manipulated by designer
    public const int defaultDamage = 5;
    public const float defaultCooldown = 0.4f;
    public float slingSpeed = 100f;
    public float slingDuration = .2f;

    // reference to Weapon GameObject to access hitbox
    public GameObject SlingshotBox;

    // for applying movement to actor
    private Rigidbody2D actorBody;

    // for coordinating techniques
    private bool selfCooling;
    private float selfCooldown = 0.6f;

    // for manipulating direciton
    HelperMethods helper;

    // MUST BE CALLED AFTER ADD COMPONENT
    public override void Initialize (int damage = defaultDamage, float cooldown = defaultCooldown) {

        // obtain reference to weapon GameObject. <<<<< IT MUST BE THE 0th CHILD >>>>
        SlingshotBox = transform.GetChild(0).gameObject;

        // obtain reference to rigidBody for moving player
        actorBody = gameObject.GetComponent<Rigidbody2D>();

        // call overloaded initialize from Technique
        base.Initialize(defaultDamage, defaultCooldown);

        helper = GetComponent<HelperMethods>();
        
    }

    // When called, the referenced GameObject will stop moving, slide back, then shoot forward with a damaging hitbox.
    public override void Act()
    {
        if (!techsCooling && !selfCooling)
        {


            // do movement and coordination
            StartCoroutine(HandleSlingShot(slingDuration));
        }

    }

    // sets the weapon object's tag to upgrade
    public override void SetUpgrade(string newUpgrade) {
        upgrade = newUpgrade;

        SlingshotBox.tag = newUpgrade;
    }
    
    private void move() {

        Vector2 currPosition = transform.position;
        Vector2 displacement = transform.up * Time.fixedDeltaTime * slingSpeed;
        currPosition += displacement;
        actorBody.MovePosition(currPosition);
    
    }

    IEnumerator HandleSlingShot(float duration)
    {
        ParticleSystem.MainModule colorSystem = SlingshotBox.GetComponentInChildren<ParticleSystem>().main;


        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        switch (SlingshotBox.tag)
        {
            case "UpgradeNone": // Purple
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(new Color(0.5419722f, 0f, 1f, 1f));
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;


                break;
            case "UpgradeSpicy": // Red
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(Color.red);
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                colorKeys[1].color = new Color(0.937255f, 0.6235294f, 0.1372549f, 1f);
                colorKeys[1].time = 0.25f;

                break;
            case "UpgradeSour": // Green
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(Color.green);
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                colorKeys[1].color = new Color(0f, 1f, 0.5668461f, 1f);
                colorKeys[1].time = 0.25f;

                break;
            case "UpgradeRock": // Yellow
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.966906f, 0f, 1f));
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;



                break;
            case "UpgradeKnockback": // Black
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(Color.black);
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;


                break;
            case "UpgradeJello": //Pink
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0f, 0.7753048f, 1f));
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                break;
        }


        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;

        alphaKeys[1].alpha = 0.5f;
        alphaKeys[1].time = 0.7f;

        gradient.SetKeys(colorKeys, alphaKeys);

        colorSystem.startColor = gradient;

        // set inter-technique coordination variables
        techsCooling = true;
        selfCooling = true;

        float t = 0.0f;

        // activate protective&damaging hitbox. 
        SlingshotBox.SetActive(true);

        // lock movement
        moveLock = true;
        
        // set direction
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, helper.CursorAngle().eulerAngles.z - 90f));

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

        // reset angle
        transform.rotation = Quaternion.identity;

        // unlock movement
        moveLock = false;
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
