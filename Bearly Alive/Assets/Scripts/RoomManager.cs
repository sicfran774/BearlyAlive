using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

/*  Room generation must consist of a few things:
 *  1.  Where/what kind of enemies spawn
 *  2.  Entrance of room/possible exits
 *  3.  A "room manager" that knows when/where a room CAN be generated
 *  - Use a "spawn manager" which reads a JSON file that contains location/type of each enemy in that room
 *  --> allows for enemies to spawn later than the beginning of the generation
 */

public class RoomManager : MonoBehaviour
{
    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [Space(20)]

    [SerializeField] private bool playerEnteredRoom = false;
    private EnemyData[] enemyOrder;

    private void Start()
    {
        SaveEnemyLocationsIntoFile();
    }

    private void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            StartCoroutine(PlaceEnemies(enemyOrder));
        }
    }

    IEnumerator PlaceEnemies(EnemyData[] enemyOrder)
    {
        foreach(EnemyData enemy in enemyOrder)
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
        EnemyData[] data = new EnemyData[100];
        int count = 0;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            data[count] = new EnemyData(enemy.GetComponent<EnemyController>(), 0);
            Debug.Log(enemy);
            count++;
        }
        EnemyPlaceScript.SaveEnemyPlacements(data);
    }
}