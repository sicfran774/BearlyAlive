using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]

public class EnemyController : MonoBehaviour
{
    //Straightforward variables to control enemy basic attributes
    public int _MAX_HEALTH = 2;
    public float projectileSpeed = 4f;
    public HudManager hud;
    int healthRemaining;

    //TODO: Add scripts to reference for these
    public GameObject player;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    private Transform bulletSpawned;
    
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

        //Build enemy
        enemy = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        healthRemaining = _MAX_HEALTH;

        //Method called when delegate is invoked 
        GameManager.instance.onToggleUpgradeMenu += OnUpgradeMenuToggle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //direction = PointAtPlayer(player.transform);

        //We want to shoot when the enemy "sees" the player, still every few seconds though
        //Shoot();
        if(currentTime == 0) 
        {
            if(isSour)
                tickDamage();
                x++;
            if(x == 5)
                isSour = false;
        }

        if(currentTime > 0 && currentTime % 2 == 0)
        {
            if(isSpicy)
                tickDamage();
                x++;
            if(x == 3)
                isSpicy = false;
        }
        
        //See how health is doing every frame
        if(healthRemaining <= 0) 
        {
            GameManager.instance.IncreaseScore(1);
            Destroy(gameObject);
        }

        //Timer for attacks and passive damage
        if(currentTime < waitTime)
            currentTime += 1 * Time.deltaTime;

        if(currentTime >= waitTime)
            currentTime = 0;

        
            
        /*void MoveEnemy ()
        {
            //Will need to test later, requires a pathfinding script
            int currentX = Mathf.RoundToInt(transform.position.x);
            int currentY = Mathf.RoundToInt(transform.position.y);
            GetComponent<BoxCollider2D>().enabled = false;
            target.GetComponent<BoxCollider2D>().enabled = false;
            AStar astar = new AStar(new LineCastAStarCost(blockingLayer), currentX, currentY, Mathf.RoundToInt(target.position.x), Mathf.RoundToInt(target.position.y));
            astar.findPath();
            AStarNode2D nextStep = (AStarNode2D)astar.solution[1];

            //GetComponent<BoxCollider2D>().enabled = true;
            target.GetComponent<BoxCollider2D>().enabled = true;

            int xDir = nextStep.x - currentX;
            int yDir = nextStep.y - currentY;

            AttemptMove <Player> (xDir, yDir);
        }*/
    }

    // Handles Enemy's objects trigger collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit with bullet, damage the enemy
        if(collision.gameObject.tag == "Bullet")
        {
            healthRemaining--;
            if(healthRemaining <= 0)
            {
                //Increase score when enemy health is <=0
                GameManager.instance.IncreaseScore(1);
                //refresh the HUD
                //hud.refresh();

                Destroy(gameObject);

            }
        }

        //If the enemy touches something spicy or sour, will become poisoned or burning

        //TODO: Change to random chance and not guarantee
        if(collision.tag == "Player")
        {
            if(Random.Range(1, 100) <= 11)
            {
                isSour = true;
            } else {
                isSour = false;
            }
        }

        if(collision.tag == "Spicy")
        {
            if(Random.Range(1, 100) <= 11)
            {
                isSpicy = true;
            } else {
                isSpicy = false;
            }

        }
    }

    //Deal damage as per the timer given by each enemies' wait time variable
    void tickDamage()
    {
        healthRemaining--;
    }

    //Handle what happens when upgrade menu is toggled  
    void OnUpgradeMenuToggle(bool active)
    {
        shot = !active;

        //Enemy movement in slow motion
        Time.timeScale = 0.1f;
   
        //Reset to normal speed when upgrade menu is exited 
        if(active == false)
        {
            Time.timeScale = 1f;
        }
    }

    //Allows the enemy to face the player as it tracks them
    private Vector2 PointAtPlayer(Transform player)
    {
        Vector3 targ = player.position;
        targ.z = 0f;
        Vector3 objectPos = transform.position;

        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        angle = (Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg) - 90f;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        return new Vector2(targ.x, targ.y);
    }

    //A basic shoot function that will be used for other attacks in the future
    public void Shoot()
    {
        shot = true;

        bulletSpawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletSpawned.rotation = transform.rotation;
        bulletSpawned.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed * 10;
    }
}
