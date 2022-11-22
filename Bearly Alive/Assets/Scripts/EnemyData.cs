using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public string enemyType;
    public float[] position;
    public float spawnTimeForNextEnemy;

    public EnemyData(GameObject enemy, float spawnTime)
    {
        enemyType = "enemyOne";

        //Set spawn position of enemy
        position = new float[2];
        position[0] = enemy.transform.position.x;
        position[1] = enemy.transform.position.y;

        //Set spawn time for the "next in-order" enemy
        spawnTimeForNextEnemy = spawnTime;
    }
}
