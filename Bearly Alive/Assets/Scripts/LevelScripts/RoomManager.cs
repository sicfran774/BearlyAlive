using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

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
 *  
 *  using https://www.boristhebrave.com/2020/09/12/dungeon-generation-in-binding-of-isaac/
 *        https://www.boristhebrave.com/permanent/20/09/isaac_gen/gen.js
 */

public class RoomManager : MonoBehaviour
{
    [Header("Level Name")]
    public string levelName;

    public int minRooms = 7;
    public int maxRooms = 15;
    public GameObject roomOne;

    [Space(20)]

    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [Space(20)]

    [SerializeField] public bool playerEnteredRoom = false;
    private List<EnemyDataJson> enemyOrder;

    [SerializeField]private int roomCount = 0;
    private int[] rooms;
    private List<int> endRooms;
    private Queue<int> cellQueue;

    System.Random rand = new System.Random();

    private void Start()
    {
        //Enemy spawning
        //SaveEnemyLocationsIntoFile();
        //enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/levelOne.json");

        //Level generation
        rooms = new int[150];
        endRooms = new List<int>();
        cellQueue = new Queue<int>();
        //Initialize all cells to be zero, any non zero element means there is a room
        for (int i = 0; i < 150; i++)
        {
            rooms[i] = 0;
        }
        
        StartCoroutine(BeginLevelGeneration());
    }

    private void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            StartCoroutine(PlaceEnemies(enemyOrder));
        }
    }

    IEnumerator BeginLevelGeneration()
    {
        while (roomCount < minRooms)
        {
            for (int i = 0; i < 150; i++)
            {
                rooms[i] = 0;
            }
            endRooms.Clear();
            cellQueue.Clear();
            visitCell(45); //This is the origin cell
            GenerateLevel();
            yield return null;
        }
    }

    void GenerateLevel()
    {
        while(cellQueue.Count > 0)
        {
            int i = cellQueue.Dequeue();
            int x = i % 10;
            var created = false;
            if (x > 1) created |= visitCell(i - 1);
            if (x < 9) created |= visitCell(i + 1);
            if (x > 20) created |= visitCell(i - 10);
            if (x < 70) created |= visitCell(i + 10);
            if(!created)
            {
                endRooms.Add(i);
            }
        }
    }

    int neighborCount(int i)
    {
        return rooms[i - 10] + rooms[i - 1] + rooms[i + 1] + rooms[i + 10];
    }

    bool visitCell(int i)
    {
        print("visiting cell " + i);
        //If this cell already has a room, return false
        if (rooms[i] != 0) return false;

        int neighbors = neighborCount(i);

        //If this cell has more than one neighbor, return false
        if (neighbors > 1) return false;

        //If the number of rooms exceed max room count, return false
        if (roomCount >= maxRooms) return false;

        //50% chance to NOT become a room (second condition is for creating the origin room)
        if (rand.Next(0, 2) == 0 && i != 45) return false;

        //Else, queue the cell and make it a room
        cellQueue.Enqueue(i);
        rooms[i] = 1;
        ++roomCount;

        //WIP
        CreateRoomInScene(i);

        return true;
    }

    void CreateRoomInScene(int i)
    {
        float x = (i % 10) * 10;
        float y = (i / 10) * 10;
        GameObject newRoom = Instantiate(roomOne);
        newRoom.transform.position = new Vector2(x, y);
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