using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyController))]

public class EnemyController : MonoBehaviour
{
    //Variables
    public int _MAX_HEALTH = 2;
    public float projectileSpeed = 4f;
    public HudManager hud;
    int healthRemaining;

    //TODO: Add scripts to reference for these
    public GameObject player;
    public GameObject bullet;
    public GameObject bulletSpawnPoint;
    private Transform bulletSpawned;

    private Vector2 direction;
    private float angle;

    public float waitTime;
    private float currentTime;
    private bool shot;

    Rigidbody2D enemy;
    Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        bulletSpawnPoint = this.gameObject;

        enemy = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        healthRemaining = _MAX_HEALTH;

        //Method called when delegate is invoked 
        GameManager.instance.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        //TODO: Find a spawn location for bullet in 2D
        //bulletSpawnPoint = GameObject.Find()


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction = PointAtPlayer(player.transform);

        if(currentTime == 0)
            Shoot();

        if(shot && currentTime < waitTime)
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
    }

    //Handle what happens when upgrade menu is toggled  
    void OnUpgradeMenuToggle(bool active)
    {
        //Disable enemy fire 
        shot = !active;

        //Todo: Disable enemy movement
    }

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

    public void Shoot()
    {
        shot = true;

        bulletSpawned = Instantiate(bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        bulletSpawned.rotation = transform.rotation;
        bulletSpawned.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed * 10;
    }
}
