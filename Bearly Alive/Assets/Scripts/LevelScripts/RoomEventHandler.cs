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

    [SerializeField] private bool playerEnteredRoom = false; //This variable is used for one time spawning
    [SerializeField] private bool playerInRoom = false;
    [SerializeField] private bool noEnemiesRemaining = false;
    [SerializeField] private bool rewarded = false;

    private List<EnemyDataJson> enemyOrder;

    void Start()
    {
        //enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/" + levelName + ".json");
    }

    void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            playerInRoom = true;
            //StartCoroutine(PlaceEnemies(enemyOrder));
        }

        if (playerInRoom)
        {
            noEnemiesRemaining = CheckIfRoomIsClear();
        }
        
        if (noEnemiesRemaining && playerInRoom && !rewarded)
        {
            DropLoot();
        }
    }

    bool CheckIfRoomIsClear()
    {
        //If the room is clear, return true -- else return false
        return (transform.childCount == 0) ? true : false;
    }

    void DropLoot()
    {
        Debug.Log("Room cleared, dropping loot!");
        rewarded = true;
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
            Debug.Log("Player entered room!");
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
            Debug.Log("Player left room!");
            playerInRoom = false;
        }
    }
}
