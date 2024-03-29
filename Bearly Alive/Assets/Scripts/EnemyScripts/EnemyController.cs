using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]

public class EnemyController : MonoBehaviour
{
    //Straightforward variables to control enemy basic attributes
    public float _MAX_HEALTH = 2f;
    public float projectileSpeed = 4f;
    public HudManager hud;
    float healthRemaining;

    //Bullet things
    public GameObject player;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    private Transform bulletSpawned;

    //Enemy shooting at player
    public float shootingRange;
    public float fireRate;
    private float nextFireTime;

    //Help get enemy angle and direction for attacks
    private Vector2 direction;
    private float angle;

    //A timer originally used for shots, but now used for sour and spice damage
    public float waitTime;
    private float currentTime;
    private bool shot;

    //IF enemy is poisoned or burning
    private bool isSour = false;
    private bool isSpicy = false;

    Rigidbody2D enemy;
    Collider2D coll;

    private int x;

    // Start is called before the first frame update
    void Start()
    {
        //Find player and initialize bullet spawnpoint
        player = GameObject.FindWithTag("Player");
        bulletSpawnPoint = this.gameObject;

        GetComponentInChildren<ParticleSystem>().Stop();

        //Build enemy
        enemy = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        healthRemaining = _MAX_HEALTH;

        //Method called when delegate is invoked 
        UpgradeUI.instance.onToggleUpgradeMenu += OnUpgradeMenuToggle;
        TechniqueUIManager.instance.onToggleWeaponMenu += OnWeaponMenuToggle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletSpawnPoint.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }

        //We want to shoot when the enemy "sees" the player, still every few seconds though
        //Shoot();
        if (currentTime == 0)
        {
            if (isSour)
            {
                tickDamage("Sour");
                x++;
            }

            if (x == 5)
            {
                isSour = false;
                GetComponentInChildren<ParticleSystem>().Stop();
            }

        }

        if (currentTime == 0)
        {
            if (isSpicy)
            {
                tickDamage("Spicy");
                x++;
            }

            if (x == 3)
            {
                isSpicy = false;
                GetComponentInChildren<ParticleSystem>().Stop();
            }

        }

        //See how health is doing every frame
        if (healthRemaining <= 0)
        {
            GameManager.instance.IncreaseScore(1);
            Destroy(gameObject);
        }

        //Timer for attacks and passive damage
        if (currentTime < waitTime)
            currentTime += 1 * Time.deltaTime;

        if (currentTime >= waitTime)
            currentTime = 0;
    }

    // Handles Enemy's objects trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit with bullet, damage the enemy
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            healthRemaining--;
            HitSound();
        }

        if (collision.gameObject.name == "Boomerang(Clone)")
        {
            healthRemaining -= 0.5f;
            HitSound();
        }

        if (collision.gameObject.name == "Sword(Clone)")
        {
            healthRemaining--;
            HitSound();
        }

        if (collision.gameObject.name == "SlingshotBox")
        {
            healthRemaining -= 3;
            HitSound();
        }

        if (collision.gameObject.name == "Whip(Clone)")
        {
            healthRemaining -= 3;
            HitSound();
        }

        if (collision.gameObject.name == "EnemyBullet" && collision.tag == "UpgradeNone") 
        {
            healthRemaining--;
            HitSound();
        }

        //If the enemy touches something spicy or sour, will become poisoned or burning
        //If the enemy is affected by an explosion, push the enemy a certain distance

        //TODO: Change to random chance and not guarantee
        if (collision.tag == "UpgradeSour")
        {
            if (Random.Range(1, 100) <= 25)
            {
                isSour = true;
            }
            else
            {
                isSour = false;
            }
        }

        if (collision.tag == "UpgradeSpicy")
        {
            if (Random.Range(1, 100) <= 25)
            {
                isSpicy = true;
            }
            else
            {
                isSpicy = false;
            }
        }

        if (collision.tag == "UpgradeKnockback")
        {
            if (Random.Range(1, 100) <= 2) {
                shootingRange = 1f;
            }

            healthRemaining--;
        }
    }

    //Deal damage as per the timer given by each enemies' wait time variable
    void tickDamage(string effect)
    {

        ParticleSystem.MainModule colorSystem = GetComponentInChildren<ParticleSystem>().main;


        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

        switch (effect)
        {
            case "Spicy":
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(Color.red);
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                colorKeys[1].color = new Color(0.937255f, 0.6235294f, 0.1372549f, 1f);
                colorKeys[1].time = 0.25f;

                break;

            case "Sour":
                colorSystem.startColor = new ParticleSystem.MinMaxGradient(Color.green);
                colorKeys[0].color = colorSystem.startColor.color;
                colorKeys[0].time = 0f;

                colorKeys[1].color = new Color(0f, 1f, 0.5668461f, 1f);
                colorKeys[1].time = 0.25f;

                break;

        }

        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;

        alphaKeys[1].alpha = 0.5f;
        alphaKeys[1].time = 0.7f;

        gradient.SetKeys(colorKeys, alphaKeys);

        colorSystem.startColor = gradient;

        GetComponentInChildren<ParticleSystem>().Play();

        healthRemaining--;
    }

    //Handle what happens when upgrade menu is toggled  
    void OnUpgradeMenuToggle(bool active)
    {
        shot = !active;

        //Enemy movement in slow motion
        Time.timeScale = 0.1f;

        //Reset to normal speed when upgrade menu is exited 
        if (active == false)
        {
            Time.timeScale = 1f;
        }

    }

    //Handle what happens when weapon menu is toggled  
    void OnWeaponMenuToggle(bool active)
    {
        shot = !active;

        //Enemy movement in slow motion
        Time.timeScale = 0.1f;

        //Reset to normal speed when upgrade menu is exited 
        if (active == false)
        {
            Time.timeScale = 1f;
        }

    }


    //Allows the enemy to face the player as it tracks them
    /*private Vector2 PointAtPlayer(Transform player)
    {
        Vector3 targ = player.position;
        targ.z = 0f;
        Vector3 objectPos = transform.position;

        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        //angle = (Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg) - 90f;

        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        return new Vector2(targ.x, targ.y);
    }*/

    //A basic shoot function that will be used for other attacks in the future
    public void Shoot()
    {
        shot = true;

        bulletSpawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletSpawned.rotation = transform.rotation;
        bulletSpawned.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed * 10;
    }

    void HitSound()
    {
        SoundManager.instance.playHitmarkerSound();
    }
}
