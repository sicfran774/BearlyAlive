using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomEventHandler : MonoBehaviour
{
    [Header("Level Name")]
    public string levelName;

    [Header("Player Health Gain")]
    int healthGain = 10;

    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [Header("Types of Upgrades")]
    public GameObject jellyInfusion;
    public GameObject malicAcid;
    public GameObject popRocks;
    public GameObject rockCandy;
    public GameObject tajinRubdown;
    public GameObject[] upgrades;

    [Header("Types of Techniques")]
    public GameObject boomerang;
    public GameObject chiSpit;
    public GameObject slash;
    public GameObject slingShot;
    public GameObject whip;
    public GameObject[] techniques;

    [SerializeField] private bool playerEnteredRoom = false; //This variable is used for one time spawning
    [SerializeField] private bool playerInRoom = false;
    [SerializeField] private bool noEnemiesRemaining = false;
    [SerializeField] private bool rewarded = false;
    [SerializeField] public bool bossRoom = false;

    private List<EnemyDataJson> enemyOrder;
    private List<GameObject> tempWalls;
    private RoomManager roomManager;
    private PlayerController player;
    private int index;
    

    void Start()
    {
        //enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/" + levelName + ".json");
        tempWalls = new List<GameObject>();
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        upgrades = new GameObject[] { jellyInfusion, malicAcid, popRocks, rockCandy, tajinRubdown };
        techniques = new GameObject[] { boomerang, chiSpit, slash, slingShot, whip};
        AstarPath.active.Scan();
        
    }

    void LateUpdate()
    {
        if (playerEnteredRoom)
        {
            PlayerEnteredRoom();
        }

        if (playerInRoom)
        {
            SetCurrentRoom();
            noEnemiesRemaining = CheckIfRoomIsClear();
            ColorCurrentRoom(roomManager.playerCurrentRoom.name + "image", new Color(256, 256, 256, 0.75f));
            EnableEnemies();
        }
        else if (rewarded)
        {
            ColorCurrentRoom(transform.parent.name + "image", Color.gray);
        }
        else if(bossRoom)
        {
            ColorCurrentRoom(transform.parent.name + "image", new Color(0, 0, 0, 0.75f));
        }
        else
        {
            ColorCurrentRoom(transform.parent.name + "image", new Color(0, 0, 0, 0.75f));
            DisableEnemies();
        }
        
        if (noEnemiesRemaining && playerInRoom && !rewarded && index != 45 || (index == 45 && !GameObject.Find("DialogueBox") && !rewarded)) //If player cleared room OR its origin room
        {
            RemoveWalls();
            if (bossRoom)
            {
                WinCondition();
            }

            DropLoot();
        } 
    }

    void PlayerEnteredRoom()
    {
        playerEnteredRoom = false;
        playerInRoom = true;

        SetCurrentRoom();
        index = Int32.Parse(roomManager.playerCurrentRoom.name);

        CloseWalls();
        AstarPath.active.Scan();
        //StartCoroutine(PlaceEnemies(enemyOrder));
        ActivateEnemyMovement();
    }

    bool CheckIfRoomIsClear()
    {
        //If the room is clear, return true -- else return false
        return (transform.childCount == 0) ? true : false;
    }

    void SetCurrentRoom()
    {
        //This converts the name (which is just the cell number) so that we can use it later to generate walls in the correct position
        roomManager.playerCurrentRoom = transform.parent.gameObject;
    }

    void ActivateEnemyMovement()
    {
        foreach(Transform enemyTransform in transform)
        {
            if (!bossRoom)
            {
                GameObject enemy = enemyTransform.gameObject;
                AIDestinationSetter enemyAI = enemy.GetComponent<AIDestinationSetter>();
                SpriteFlip enemySprite = enemy.GetComponent<SpriteFlip>();
                enemyAI.target = GameObject.FindWithTag("Player").transform;
                enemySprite.player = GameObject.FindWithTag("Player").transform;
            }
        }
    }

    void DropLoot()
    {
        Debug.Log("Room cleared, dropping loot!");
        rewarded = true;
        RegainHealth();

        //Randomly generate upgrade/techniques
        System.Random rand = new System.Random();
        int randUpgradeNum = rand.Next(0, upgrades.Length);
        int randTechniqueNum = rand.Next(0, techniques.Length);

        GameObject upgrade;
        GameObject technique;

        if (index != roomManager.lootRoomIndex && index != 45 && rand.Next(0, 2) == 0) //50% chance to drop anything
        {
            if(rand.Next(0,3) != 0) //66% chance to be an upgrade
            {
                upgrade = Instantiate(upgrades[randUpgradeNum], transform.parent);
                upgrade.transform.position = new Vector2(upgrade.transform.position.x, upgrade.transform.position.y);
            }
            else //33% chance to be a technique
            {
                technique = Instantiate(techniques[randTechniqueNum], transform.parent);
                technique.transform.position = new Vector2(technique.transform.position.x, technique.transform.position.y);
            }
            
        }
        else if(index == roomManager.lootRoomIndex || index == 45) //Drop in loot rooms and origin room
        {
            upgrade = Instantiate(upgrades[randUpgradeNum], transform.parent);
            upgrade.transform.position = new Vector2(upgrade.transform.position.x, upgrade.transform.position.y);
            technique = Instantiate(techniques[randTechniqueNum], transform.parent);
            technique.transform.position = new Vector2(technique.transform.position.x + 5, technique.transform.position.y);
        }
    }

    void RegainHealth()
    {
        print("Player gained " + healthGain + "HP.");
        print("Current HP: " + player.healthBar.currentHealth);
        player.healthBar.ReceivedHealth(healthGain);
    }

    void CloseWalls()
    {
        //Add walls to each entrance to trap player, then add each wall to an array to destroy later
        tempWalls.Add(roomManager.AddWall(index, 0));
        tempWalls.Add(roomManager.AddWall(index, 1));
        tempWalls.Add(roomManager.AddWall(index, 2));
        tempWalls.Add(roomManager.AddWall(index, 3));
    }

    void RemoveWalls()
    {
        foreach(GameObject wall in tempWalls)
        {
            Destroy(wall);
        }
    }

    void ColorCurrentRoom(string currentRoom, Color color)
    {
        GameObject.Find(currentRoom).GetComponent<Image>().color = color;
    }

    IEnumerator PlaceEnemies(List<EnemyDataJson> enemyOrder)
    {
        foreach (EnemyDataJson enemy in enemyOrder)
        {
            if (enemy == null)
            {
                continue;
            }

            GameObject newEnemy = SpawnEnemy(enemy.enemyType);
            newEnemy.transform.position = new Vector2(enemy.position[0], enemy.position[1]);

            yield return new WaitForSeconds(enemy.spawnTimeForNextEnemy);
        }
    }
    GameObject SpawnEnemy(string type)
    {
        switch (type)
        {
            case "enemyOne":
                return Instantiate(enemyOne, transform);
            default:
                return Instantiate(enemyOne, transform);
        }
    }

    void EnableEnemies()
    {
        foreach (Transform enemy in transform)
        {
            enemy.gameObject.SetActive(true);
        }
    }

    void DisableEnemies()
    {
        foreach(Transform enemy in transform)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    void WinCondition()
    {
        GameManager.instance.GetComponent<AudioSource>().Stop();
        SceneManager.LoadScene("Victory");
    }


    void SaveEnemyLocationsIntoFile()
    {
        List<EnemyData> data = new List<EnemyData>();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            data.Add(new EnemyData(enemy, 0));
            Debug.Log(enemy);
        }
        EnemyPlaceScript.SaveEnemyPlacements(data);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !noEnemiesRemaining)
        {
            Debug.Log("Player entered new room!");
            playerEnteredRoom = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRoom = false;
        }
    }
}
