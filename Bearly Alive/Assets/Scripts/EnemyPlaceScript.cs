using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class EnemyPlaceScript
{
    public static void SaveEnemyPlacements(EnemyData[] enemyOrder)
    {
        string path = Application.persistentDataPath + "/levelOne.json";

        string json = JsonConvert.SerializeObject(enemyOrder, Formatting.Indented);
        System.IO.File.WriteAllText(path, json);
    }
}
