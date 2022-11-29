using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : Technique
{

    private GameObject whip;

    // The amount of time for slash action to finish
    public float whipDuration = 1f;

    Rigidbody2D body;
    Collider2D coll;

    // to be manipulated by designer
    public const int defaultDamage = 10;
    // Time to wait until player is able to use technique again
    public const float defaultCooldown = 1f;

    private GameObject childWhipGameObject;


    // Makes the sword hidden when it is first initializes when the game starts
    // ALWAYS call after add component
    public override void Initialize(int damage = defaultDamage, float cooldown = defaultCooldown)
    {
        base.Initialize(defaultDamage, defaultCooldown);
        whip = GameManager.instance.Whip;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        whip.SetActive(false);
        int direction = transform.position.y >= 0 ? 1 : -1;
        childWhipGameObject = Instantiate(whip, new Vector2(transform.position.x, transform.position.y + 6f), Quaternion.Euler(0f, 0f, 0f), transform);
    }

    public override void Act()
    {
        if (!techsCooling)
        {
            SoundManager.instance.playSlashSound();
            StartCoroutine(PerformWhip(whipDuration));
            startCooling();
        }
    }


    public override void SetUpgrade(string newUpgrade)
    {
        // TODO
    }



    // When the player activates the Slash Technique, sword object is re-enabled
    // When sword is re-enabled, function is called setting up the rigidbody and collider
    // and performing the circular slash
    private void OnEnable()
    {
        //speed = slashSpeed;
        body = GetComponentInChildren<Rigidbody2D>();
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
    IEnumerator PerformWhip(float duration)
    {

        childWhipGameObject.SetActive(true);
     
        cursorLock = true;

        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            yield return null;
        }
        cursorLock = false;
        childWhipGameObject.SetActive(false);

    }
}
