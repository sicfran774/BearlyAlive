using System.Collections;
using System.Collections.Generic;
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
    public GameObject enemy;

    [Space(20)]

    private bool playerEnteredRoom = false;
    private string[] enemyOrder;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (playerEnteredRoom)
        {
            playerEnteredRoom = false;
            StartCoroutine(PlaceEnemies(enemyOrder));
        }
    }

    IEnumerator PlaceEnemies(string[] enemyOrder)
    {
        yield return null;
    }
}
