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
    [Header("Level Name")]
    public string levelName;

    [Space(20)]

    //must import specific enemy types
    [Header("Types of Enemies")]
    public GameObject enemyOne;

    [Space(20)]

    [SerializeField] public bool playerEnteredRoom = false;
    private List<EnemyDataJson> enemyOrder;

    private void Start()
    {
        //SaveEnemyLocationsIntoFile();
        enemyOrder = EnemyPlaceScript.LoadEnemyData(Application.persistentDataPath + "/levelOne.json");
    }

    private void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            StartCoroutine(PlaceEnemies(enemyOrder));
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