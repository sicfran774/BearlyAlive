using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    //public static RoomManager instance = null;

    public int minRooms;
    public int maxRooms;
    public int maxLootRooms;

    [Space(20)]
    [Header("Types of Rooms")]
    public List<GameObject> roomPrefabList;
    public GameObject lootRoom;
    public GameObject bossRoom;

    [Header("Basic Room Pieces")]
    public GameObject emptyRoom;
    public GameObject entranceWall;

    [Space(20)]
    [Header("Map Generation Images")]
    public GameObject roomImage;
    public GameObject lootImage;
    public GameObject bossImage;

    [Header("Canvas Map")]
    public GameObject map;
    public GameObject loadingScreen;

    public LayerMask ignoreLayer;

    [SerializeField] private int roomCount = 0;
    private int[] rooms;
    private List<int> roomList;
    private List<int> endRooms;
    private Queue<int> cellQueue;
    private GameObject roomsParent;


    public int bossRoomIndex { get; set; }
    public int lootRoomIndex { get; set; }

    private AstarData data;

    public GameObject playerCurrentRoom { get; set; }

    private bool addRoomsToGrid;
    public bool doneGeneratingRooms = false;

    private const float OriginOffsetX = -500;
    private const float OriginOffsetY = -200;

    private const float MapOffsetX = 215;
    private const float MapOffsetY = -300;

    System.Random rand = new System.Random();

    /*private void Awake()
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
    }*/

    private void Start()
    {
        //Enemy spawning
        //SaveEnemyLocationsIntoFile();
        

        //Level generation
        roomsParent = GameObject.Find("Rooms");
        rooms = new int[maxRooms * 10];
        roomList = new List<int>();
        endRooms = new List<int>();
        cellQueue = new Queue<int>();

        
        StartCoroutine(BeginLevelGeneration());
    }

    private void Update()
    {
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
            roomList.Clear();
            endRooms.Clear();
            cellQueue.Clear();
            roomCount = 0;
            visitCell(45); //This is the origin cell
            GenerateLevel();

            if(roomCount >= minRooms)
            {
                PickBossRoom(endRooms);
                PickLootRoom(maxLootRooms);
                StartCoroutine(CreateRoomsInScene());
                StartCoroutine(CloseWalls());
                RenderMap();

                yield break;
            }

            //If it reaches this point then min amount of rooms not reached, must try again
            RestartRoomGen();
            yield return null;

        }
    }

    void RestartRoomGen()
    {
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Wall"))
        {
            Destroy(room);
        }

        //get all active graph
        data = AstarPath.active.data;

        //empty all data graph
        System.Array.Clear(data.graphs, 0, data.graphs.Length);
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
        roomList.Add(i);

        return true;
    }

    IEnumerator CreateRoomsInScene()
    {
        //Create GameObjects for every room and place them in level
        foreach (int i in roomList)
        {
            float x = (i % 10) * 100 + OriginOffsetX;
            float y = (i / 10) * 50 + OriginOffsetY;
            GameObject newRoom;

            CreateGridGraph(x, y);

            if (i == 45)
            {
                newRoom = Instantiate(emptyRoom); //Origin will always be empty room
            } 
            else if (i == bossRoomIndex)
            {
                newRoom = Instantiate(bossRoom);
                newRoom.GetComponentInChildren<RoomEventHandler>().bossRoom = true;
            }
            else if (i == lootRoomIndex)
            {
                newRoom = Instantiate(lootRoom);
            }
            else
            {
                newRoom = Instantiate(PickRandomRoomPrefab()); //Other rooms can be any variation
            }

            newRoom.transform.position = new Vector2(x, y);
            newRoom.tag = "Wall";
            newRoom.name = i.ToString();

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
            yield return null;
        }
    }

    void CreateGridGraph(float x, float y)
    {
        data = AstarPath.active.data;
        GridGraph gg = data.AddGraph(typeof(GridGraph)) as GridGraph;
        int width = 85, depth = 35;
        float nodeSize = 1.2f;
        gg.center = new Vector3(x, y, 0);
        gg.SetDimensions(width, depth, nodeSize);
        gg.is2D = true;
        gg.collision.type = ColliderType.Sphere;
        gg.collision.use2D = true;
        gg.collision.diameter = 3;
        gg.collision.mask = ignoreLayer;
    }

    GameObject PickRandomRoomPrefab()
    {
        int num = rand.Next(0, roomPrefabList.Count);
        return roomPrefabList[num];
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

    void PickBossRoom(List<int> endRooms)
    {
        int randomEndRoom = rand.Next(0, endRooms.Count);
        bossRoomIndex = endRooms[randomEndRoom];
        print("Boss Room: " + bossRoomIndex);
    }

    void PickLootRoom(int maxLootRooms)
    {
        for(int i = 0; i < maxLootRooms; i++)
        {
            while (true)
            {
                int lootRoom = rand.Next(0, roomCount);
                if (roomList[lootRoom] != bossRoomIndex && roomList[lootRoom] != 45)
                {
                    lootRoomIndex = roomList[lootRoom];
                    break;
                }
            }
        }
        print("Loot Room: " + lootRoomIndex);
    }

    void RenderMap()
    {
        List<GameObject> cellImages = new List<GameObject>();
        float x, y;
        int mostRightRoom = 0, mostBelowRoom = 90;
        foreach (int i in roomList)
        {
            mostRightRoom = Math.Max(mostRightRoom % 10, i % 10);
            mostBelowRoom = Math.Min(mostBelowRoom / 10, i / 10) * 10;
            print(mostBelowRoom);

            x = (i % 10) * 40 * map.transform.localScale.x;
            y = (i / 10) * 20 * map.transform.localScale.y;

            GameObject cellImage;

            if(i == lootRoomIndex)
            {
                cellImage = Instantiate(lootImage);
            }
            else if(i == bossRoomIndex)
            {
                cellImage = Instantiate(bossImage);
            }
            else
            {
                cellImage = Instantiate(roomImage);
            }

            cellImage.transform.SetParent(map.transform, false);
            cellImage.transform.localScale = map.transform.localScale;
            cellImage.transform.localPosition = new Vector2(cellImage.transform.localPosition.x + x, cellImage.transform.localPosition.y + y);
            cellImage.name = i + "image";
            cellImages.Add(cellImage);
        }
        foreach (GameObject cellImage in cellImages)
        {
            int moveRoomsLeft = mostRightRoom * 25; 
            int moveRoomsDown = mostBelowRoom * 3;

            cellImage.transform.localPosition = new Vector2(cellImage.transform.localPosition.x - moveRoomsLeft, cellImage.transform.localPosition.y - moveRoomsDown);
        }

        StartCoroutine(RemoveLoadingScreen());
    }

    IEnumerator RemoveLoadingScreen()
    {
        yield return new WaitForSeconds(1.5f);
        map.transform.GetComponentInParent<DialogueUI>().ShowDialogue(map.transform.GetComponentInParent<DialogueUI>().testDialogue);
        loadingScreen.SetActive(false);
    }

    public GameObject AddWall(int i, int dir) //dir --> 0, left; 1, right; 2, below; 3, above
    {
        float x = (i % 10) * 100 + OriginOffsetX;
        float y = (i / 10) * 50 + OriginOffsetY;
        GameObject wall = Instantiate(entranceWall, roomsParent.transform);
        switch (dir)
        {
            case 0:
                wall.transform.position = new Vector2(x - 45, y);
                wall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
                wall.tag = "Wall";
                wall.name = "cell " + i + " left";
                break;
            case 1:
                wall.transform.position = new Vector2(x + 50, y);
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
        return wall;
    }

    
    IEnumerator AddRoomsToGrid()
    {
        yield return new WaitForSeconds(1);
        Debug.LogWarning("Invoked AddRoomsToGrid");

        addRoomsToGrid = false;
        roomsParent = GameObject.Find("Rooms");
        foreach (GameObject room in GameObject.FindGameObjectsWithTag("Wall"))
        {
            room.transform.parent = roomsParent.transform;
        }
    }
}