using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int _MAX_HEALTH = 2;
    public HudManager hud;
    int healthRemaining;

    Rigidbody2D enemy;
    Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        healthRemaining = _MAX_HEALTH;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        public void MoveEnemy ()
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
        }
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



}
