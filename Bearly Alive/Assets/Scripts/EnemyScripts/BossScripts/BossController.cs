using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public bool isMoving { get; private set; }
    public bool isJumping { get; private set; }
    public bool isShooting { get; private set; }
    public bool isDead { get; private set; }
    public float slamRange = 15f;
    public float range = 25f;
    public float shootingRange = 20f;
    private bool shot;
    public float timeBTWShots = 30f;
    public Transform shootPos;

    public Vector2 direction;
    public float speed = 1f;
    private Rigidbody2D rigidbody;
    public Transform transform;
    public GameObject player;
    public Collider2D coll;
    public int MAX_HEALTH = 30;
    public int healthRemaining;
    public GameObject bullet;
    private Vector2 velocity;

    public AnimatedSprite moving;
    public DeathAnimation dying;
    public SpriteRenderer spriteRenderer;

    private Sprite boss;

    private int count = 0;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        boss = spriteRenderer.sprite;
        healthRemaining = MAX_HEALTH;
        enabled = false;

        isJumping = false;
        isDead = false;
        isMoving = false;
        isShooting = false;
    }

     private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }
    private void FixedUpdate()
    {
        StartCoroutine(BossMovement());
    }

    private IEnumerator BossMovement()
    {
        moving.enabled = true;
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);
        if (playerDistance <= range && !isJumping) {
            if (playerDistance <= shootingRange) {
                print("shoot");
                rigidbody.Sleep();
                isShooting = true;
                StartCoroutine(Shoot());
                Move();
                yield break;
            }
            else{
                if (playerDistance <= slamRange) {
                    
                    player.GetComponent<PlayerController>().healthBar.TookDamage(20);
                }
                Move();
                yield break;
            }
        }
        if (healthRemaining <= 0) {
            isDead = true;
            moving.enabled = false;
            dying.enabled = true;
        }
    }

    private void Move()
    {
        isMoving = true;
        moving.enabled = true;
        direction = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
        velocity.x = direction.x * speed;
        velocity.y = direction.y;

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
         if (rigidbody.Raycast(direction)) {
            direction = -direction;
         }
    }

    private void Slam()
    {
        // TODO write slam
        isJumping = true;
        print("SLAM");
        spriteRenderer.enabled = false;
        transform.position = player.transform.position;
        new WaitForSeconds(2f);
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = boss;

    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        count ++;
        yield return new WaitForSeconds(timeBTWShots);
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
        if (count == 5) {
            count = 0;
            yield break;
        }
    }

    // Handles Enemy's objects trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit with bullet, damage the enemy
        if (collision.gameObject.name == "Bullet(Clone)")
        {
            healthRemaining--;
        }

        if (collision.gameObject.name == "Boomerang(Clone)")
        {
            healthRemaining--;
        }

        if (collision.gameObject.name == "Sword(Clone)")
        {
            healthRemaining--;
        }

        if (collision.gameObject.name == "Slingshot(Clone)")
        {
            healthRemaining--;
        }

        if (collision.gameObject.name == "Whip(Clone)")
        {
            healthRemaining--;
        }
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
}
