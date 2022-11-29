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
        swordChildObject = Instantiate(sword, new Vector2(transform.position.x, transform.position.y + 3.5f), Quaternion.Euler(0f,0f,0f), transform);
    }

    public override void Act()
    {
        if (!techsCooling) {
            SoundManager.instance.playSlashSound();
            StartCoroutine(Rotate(slashDuration));
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
        swordChildObject.SetActive(true);
        cursorLock = true;

        float startRotation = transform.eulerAngles.z + 45f;
        float endRotation = startRotation - 405.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 405.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
            yield return null;
        }
        cursorLock = false;
        swordChildObject.SetActive(false);
    }

}
