using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEventHandler : MonoBehaviour
{
    [Header("Level Name")]
    public string levelName;

    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [SerializeField] public bool playerEnteredRoom = false;
    [SerializeField] public bool playerInRoom = false;
    [SerializeField] private bool noEnemiesRemaining = false;

    private List<EnemyDataJson> enemyOrder;
    private List<GameObject> enemies;
    private GameObject roomsParent;

    void Start()
    {
        enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/" + levelName + ".json");
        roomsParent = GameObject.Find("Rooms");
    }

    void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            playerInRoom = true;
            StartCoroutine(PlaceEnemies(enemyOrder));
        }

        if (playerInRoom)
        {
            UpdateEnemyListInRoom();
            noEnemiesRemaining = CheckIfRoomIsClear();
        }
        
        if (noEnemiesRemaining && playerInRoom)
        {

            DropLoot();
        }
    }

    void UpdateEnemyListInRoom()
    {
        GameObject.g
    }

    bool CheckIfRoomIsClear()
    {
        //If the room is clear, return true -- else return false
        return enemyOrder == null ? true : false;
    }

    void DropLoot()
    {

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
                return Instantiate(enemyOne, roomsParent.transform);
            default:
                return Instantiate(enemyOne, roomsParent.transform);
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
