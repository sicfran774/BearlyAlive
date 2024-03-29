/*************************************************************** 
*file: AnimatedSprite.cs 
*author: M. Tene 
*class: CS 4700 – Game Development 
*assignment: program 4 
*date last modified: 12/5/2022 
* 
*purpose: this program handles sprite animation logic for the boss, along with movement logic and execution. 
* 
****************************************************************/ 


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
    public float timeBTWShots = 1f;
    public Transform shootPos;

    public Vector2 direction;
    public float speed = 1f;
    private Rigidbody2D rigidBody;
    public Transform transform;
    public GameObject player;
    public Collider2D coll;
    public int MAX_HEALTH = 1;
    public int healthRemaining;
    public GameObject bullet;
    private Vector2 velocity;

    public AnimatedSprite moving;
    public DeathAnimation dying;
    public SpriteRenderer spriteRenderer;

    private Sprite boss;

    private int count = 0;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
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
        rigidBody.WakeUp();
    }

    private void OnDisable()
    {
        rigidBody.velocity = Vector2.zero;
        rigidBody.Sleep();
    }
    private void FixedUpdate()
    {
         if (healthRemaining <= 0) {
            isDead = true;
            rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            moving.enabled = false;
            dying.enabled = true;
            GameManager.instance.IncreaseScore(1);
            StartCoroutine(KillBoss());
        } else {
            StartCoroutine(BossMovement());
        }
      
    }

    IEnumerator KillBoss()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private IEnumerator BossMovement()
    {
        float playerDistance = Vector2.Distance(transform.position, player.transform.position);
        if (playerDistance <= range && !isJumping) {
            if (playerDistance <= shootingRange) {
                print("shoot");
                rigidBody.Sleep();
                isShooting = true;
                StartCoroutine(Shoot());
                yield return new WaitForSeconds(5f);
                Move();
                Slam();
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
       
    }

    private void Move()
    {
        isMoving = true;
        moving.enabled = true;
        direction = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
        velocity.x = direction.x * speed;
        velocity.y = direction.y;

        rigidBody.MovePosition(rigidBody.position + -velocity * Time.fixedDeltaTime / 10);
         if (rigidBody.Raycast(direction)) {
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
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = boss;
        rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);

    }

    private IEnumerator Shoot()
    {
        isShooting = true;
        count ++;
        yield return new WaitForSeconds(10);
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
        if (count == 2) {
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
            SoundManager.instance.playHitmarkerSound();
        }

        if (collision.gameObject.name == "Boomerang(Clone)")
        {
            healthRemaining--;
            SoundManager.instance.playHitmarkerSound();
        }

        if (collision.gameObject.name == "Sword(Clone)")
        {
            healthRemaining--;
            SoundManager.instance.playHitmarkerSound();
        }

        if (collision.gameObject.name == "SlingshotBox")
        {
            healthRemaining--;
            SoundManager.instance.playHitmarkerSound();
        }

        if (collision.gameObject.name == "Whip(Clone)")
        {
            healthRemaining--;
            SoundManager.instance.playHitmarkerSound();
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
