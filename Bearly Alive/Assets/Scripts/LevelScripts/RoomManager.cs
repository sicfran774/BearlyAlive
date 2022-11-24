using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.UI.Image;

/*  Room generation must consist of a few things:
 *  1.  Where/what kind of enemies spawn
 *  2.  Entrance of room/possible exits
 *  3.  A "room manager" that knows when/where a room CAN be generated
 *  - Use a "spawn manager" which reads a JSON file that contains location/type of each enemy in that room
 *  --> allows for enemies to spawn later than the beginning of the generation
 *  
 *  For room generation, I'm using one inspired by Binding of Issac and breadth-first search
 *  It uses a grid of cells, then determines if each cell will have a room in it using a queue
 *  It loops over a queue for all 4 directions (NSEW) and determines if the cell
 *  1) has more than 1 neighbor
 *  2) already has a room in that cell
 *  if not, then make that cell a room and queue it
 */

public class RoomManager : MonoBehaviour
{
    [Header("Level Name")]
    public string levelName;

    public int maxRooms;
    public GameObject roomOne;

    [Space(20)]

    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [Space(20)]

    [SerializeField] public bool playerEnteredRoom = false;
    private List<EnemyDataJson> enemyOrder;

    private int roomCount = 0;
    private int[] rooms;
    private Queue<int> cellQueue;

    System.Random rand = new System.Random();

    private void Start()
    {
        //SaveEnemyLocationsIntoFile();
        enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/levelOne.json");

        //Level generation
        cellQueue = new Queue<int>();
        rooms = new int[100];
        for(int i = 0; i < 100; i++)
        {
            rooms[i] = 0;
        }

        GenerateLevel();
    }

    private void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            StartCoroutine(PlaceEnemies(enemyOrder));
        }
    }

    void GenerateLevel()
    {
        
    }

    IEnumerator PlaceEnemies(List<EnemyDataJson> enemyOrder)
    {
        foreach(EnemyDataJson enemy in enemyOrder)
        {
            if(enemy == null)
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
        switch (type) {
            case "enemyOne": 
                return Instantiate(enemyOne);
            default:
                return Instantiate(enemyOne);
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
}