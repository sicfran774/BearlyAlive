using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boomerang : Technique
{

    private GameObject boomerang;

    // The amount of time for slash action to finish
    public float duration = 2.5f;

    public float tossSpeed = 2f;

    public float standDuration = 0.7f;

    public float returnSpeed = 2f;

    public float rotationSpeed = 100f;

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
        upgrade = newUpgrade;
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Boomerang(Clone)")
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
        boomerang.tag = upgrade;
        ParticleSystem.MainModule colorSystem = boomerang.GetComponentInChildren<ParticleSystem>().main;


        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        switch (boomerang.tag)
        {
            case "UpgradeNone": // Blue
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(Color.blue);
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                colorKeys[1].color = new Color(0f, 0.5049267f, 1f, 1f);
                colorKeys[1].time = 0.25f;

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

                colorKeys[1].color = new Color(0.9528302f, 0.9417379f, 0.5992643f, 1f);
                colorKeys[1].time = 0.25f;


                break;
            case "UpgradeKnockback": // Black
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(Color.black);
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                colorKeys[1].color = Color.white;
                colorKeys[1].time = 0.4f;


                break;
            case "UpgradeJello": //Pink
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0f, 0.7753048f, 1f));
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                colorKeys[1].color = new Color(1f, 0.4591194f, 0.8522459f, 1f);
                colorKeys[1].time = 0.25f;

                break;
        }


        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;

        alphaKeys[1].alpha = 0.5f;
        alphaKeys[1].time = 0.7f;

        gradient.SetKeys(colorKeys, alphaKeys);

        colorSystem.startColor = gradient;


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
            float angle = rotationSpeed * Time.fixedDeltaTime;

            boomerang.transform.Rotate(Vector3.forward * angle, Space.World);
            t += Time.fixedDeltaTime;

            yield return null;
        }

        // Set Boomerang's velocity to 0
        t = 0;
        boomerangRigidBody.velocity = Vector2.zero;

        // Duration for how long the boomerang should wait before returning to player
        while (t < standDuration)
        {
            //distance (in angles) to rotate on each frame. distance = speed * time
            float angle = rotationSpeed * Time.fixedDeltaTime;

            boomerang.transform.Rotate(Vector3.forward * angle, Space.World);
            t += Time.fixedDeltaTime;
            yield return null;
        }

        //  Start Boomerang to follow Player to Catch for next use
        returning = true;
        while (returning)
        {
            float angle = rotationSpeed * Time.fixedDeltaTime;

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