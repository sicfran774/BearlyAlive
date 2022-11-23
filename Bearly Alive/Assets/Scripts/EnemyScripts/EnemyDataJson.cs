using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDataJson
{
    [JsonProperty("enemyType")]
    public string enemyType { get; set; }

    [JsonProperty("position")]
    public float[] position { get; set; }

    [JsonProperty("spawnTimeForNextEnemy")]
    public float spawnTimeForNextEnemy { get; set; }

}
