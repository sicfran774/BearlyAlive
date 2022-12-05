using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : Technique
{

    private GameObject sword;

    // The amount of time for slash action to finish
    public float slashDuration = 0.5f;

    Rigidbody2D swordRigidBody;
    Collider2D coll;

    // to be manipulated by designer
    public const int defaultDamage = 10;
    // Time to wait until player is able to use technique again
    public const float defaultCooldown = 1f;

    private GameObject swordChildObject;


    // Makes the sword hidden when it is first initializes when the game starts
    // ALWAYS call after add component
    public override void Initialize(int damage = defaultDamage, float cooldown = defaultCooldown)
    {
        base.Initialize(defaultDamage, defaultCooldown);
        sword = GameManager.instance.Sword;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        sword.SetActive(false);
        int direction = transform.position.y >= 0 ? 1 : -1;
        swordChildObject = Instantiate(sword, new Vector2(transform.position.x, transform.position.y + 3.5f), Quaternion.Euler(0f, 0f, 0f), transform);
    }

    public override void Act()
    {
        if (!techsCooling)
        {
            SoundManager.instance.playSlashSound();
            StartCoroutine(Rotate(slashDuration));
            startCooling();
        }
    }


    public override void SetUpgrade(string newUpgrade)
    {
        upgrade = newUpgrade;
        swordChildObject.tag = upgrade;
    }



    // When the player activates the Slash Technique, sword object is re-enabled
    // When sword is re-enabled, function is called setting up the rigidbody and collider
    // and performing the circular slash
    private void OnEnable()
    {
        //speed = slashSpeed;
        swordRigidBody = GetComponentInChildren<Rigidbody2D>();
        coll = GetComponentInChildren<Collider2D>();

    }

    // When the circular slash is done, the sword object is disabled
    // This function is called, removing its rigidbody and collider
    private void OnDisable()
    {
        //sword = null;
        //coll = null;
    }


    // Coroutine that handles the circular slash technique.
    // The amount of time it takes to complete the action is 
    // based on the argument passed.
    IEnumerator Rotate(float duration)
    {

        ParticleSystem.MainModule colorSystem = swordChildObject.GetComponentInChildren<ParticleSystem>().main;


        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        switch (swordChildObject.tag)
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
        alphaKeys[1].time = 0.4f;

        gradient.SetKeys(colorKeys, alphaKeys);

        colorSystem.startColor = gradient;



        swordChildObject.SetActive(true);


        cursorLock = true;
        moveLock = true;

        float startRotation = transform.eulerAngles.z + 45f;
        float endRotation = startRotation - 405.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.fixedDeltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 405.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
            yield return null;
        }
        cursorLock = false;
        moveLock = false;
        swordChildObject.SetActive(false);
    }

}
