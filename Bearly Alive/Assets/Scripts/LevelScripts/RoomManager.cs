using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    public static RoomManager instance = null;

    [Header("Level Name")]
    public string levelName;

    public int minRooms;
    public int maxRooms;
    public GameObject roomOne;
    public GameObject entranceWall;

    [Space(20)]

    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [Space(20)]

    [SerializeField] public bool playerEnteredRoom = false;
    private List<EnemyDataJson> enemyOrder;

    [SerializeField] private int roomCount = 0;
    private int[] rooms;
    private List<int> endRooms;
    private Queue<int> cellQueue;
    private GameObject roomsParent;
    private bool addRoomsToGrid;
    private const float OriginOffsetX = -492;
    private const float OriginOffsetY = -180;

    System.Random rand = new System.Random();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //Enemy spawning
        //SaveEnemyLocationsIntoFile();
        enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/levelOne.json");

        //Level generation
        roomsParent = GameObject.Find("Rooms");
        rooms = new int[maxRooms * 10];
        endRooms = new List<int>();
        cellQueue = new Queue<int>();

        StartCoroutine(BeginLevelGeneration());
        StartCoroutine(CloseWalls());
    }

    private void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            StartCoroutine(PlaceEnemies(enemyOrder));
        }
        if (addRoomsToGrid)
        {
            StartCoroutine(AddRoomsToGrid());
        }
    }

    IEnumerator BeginLevelGeneration()
    {
        while (roomCount < minRooms)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                rooms[i] = 0;
            }
            endRooms.Clear();
            cellQueue.Clear();
            roomCount = 0;
            visitCell(45); //This is the origin cell
            GenerateLevel();

            if(roomCount >= minRooms)
            {
                yield break;
            }

            //If it reaches this point then min amount of rooms not reached, must try again
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield return null;

        }

    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        //Debug: print("visiting cell " + i);
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
        float x = (i % 10) * 100 + OriginOffsetX;
        float y = (i / 10) * 50 + OriginOffsetY;
        GameObject newRoom = Instantiate(roomOne);
        newRoom.transform.position = new Vector2(x, y);
        newRoom.tag = "Wall";
        newRoom.name = "Cell " + i ;

        if (roomsParent != null)
        {
            newRoom.transform.parent = roomsParent.transform;
        }
        else
        {
            try
            {
                roomsParent = GameObject.Find("Rooms");
                newRoom.transform.parent = roomsParent.transform;
            }
            catch 
            {
                addRoomsToGrid = true;
            }
        }
    }

    //For blocking off entrances that don't have a room in that direction
    IEnumerator CloseWalls()
    {
        for(int i = 0; i < rooms.Length; i++) //Traverse through all cells
        {
            if (rooms[i] != 0) //If cell has room
            {
                Debug.Log("Checking cell " + i);
                if (rooms[i - 1] == 0) AddWall(i, 0); //Check if left has room, if yes add wall
                if (rooms[i + 1] == 0) AddWall(i, 1); //Right
                if (rooms[i - 10] == 0) AddWall(i, 2); //Below
                if (rooms[i + 10] == 0) AddWall(i, 3); //Above
            }
            yield return null;
        }
    }

    void AddWall(int i, int dir) //dir --> 0, left; 1, right; 2, below; 3, above
    {
        float x = (i % 10) * 100 + OriginOffsetX;
        float y = (i / 10) * 50 + OriginOffsetY;
        GameObject wall = Instantiate(entranceWall, roomsParent.transform);
        switch (dir)
        {
            case 0:
                wall.transform.position = new Vector2(x - 68.3457f, y - 6.91f); //I know these numbers are scuffed... sorry
                wall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                wall.tag = "Wall";
                wall.name = "cell " + i + " left";
                break;
            case 1:
                wall.transform.position = new Vector2(x + 26.66f, y - 6.91f);
                wall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                wall.tag = "Wall";
                wall.name = "cell " + i + " right";
                break;
            case 2:
                wall.transform.position = new Vector2(x, y - 25);
                wall.tag = "Wall";
                wall.name = "cell " + i + " below";
                break;
            case 3:
                wall.transform.position = new Vector2(x, y + 20);
                wall.tag = "Wall";
                wall.name = "cell " + i + " above";
                break;
            default: break;
        }
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
                return Instantiate(enemyOne, roomsParent.transform);
            default:
                return Instantiate(enemyOne, roomsParent.transform);
        }
    }
    IEnumerator AddRoomsToGrid()
    {
        yield return new WaitForSeconds(1);
        Debug.LogWarning("Invoked AddRoomsToGrid");

        addRoomsToGrid = false;
        roomsParent = GameObject.Find("Rooms");
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Room"))
        {
            room.transform.parent = roomsParent.transform;
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