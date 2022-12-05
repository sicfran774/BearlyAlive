using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomEventHandler : MonoBehaviour
{
    [Header("Level Name")]
    public string levelName;

    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [SerializeField] private bool playerEnteredRoom = false; //This variable is used for one time spawning
    [SerializeField] private bool playerInRoom = false;
    [SerializeField] private bool noEnemiesRemaining = false;
    [SerializeField] private bool rewarded = false;

    private List<EnemyDataJson> enemyOrder;
    private List<GameObject> tempWalls;
    private RoomManager roomManager;
    private int index;
    

    void Start()
    {
        //enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/" + levelName + ".json");
        tempWalls = new List<GameObject>();
        roomManager = GameObject.Find("RoomManager").GetComponent<RoomManager>();
    }

    void LateUpdate()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            playerInRoom = true;

            SetCurrentRoom();
            index = Int32.Parse(roomManager.playerCurrentRoom.name);

            CloseWalls();
            AstarPath.active.Scan();
            //StartCoroutine(PlaceEnemies(enemyOrder));
        }

        if (playerInRoom)
        {
            SetCurrentRoom();
            noEnemiesRemaining = CheckIfRoomIsClear();
        }
        
        if (noEnemiesRemaining && playerInRoom && !rewarded)
        {
            RemoveWalls();
            DropLoot();
        }
        //print(currentRoom);
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

    void DropLoot()
    {
        Debug.Log("Room cleared, dropping loot!");
        rewarded = true;
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
