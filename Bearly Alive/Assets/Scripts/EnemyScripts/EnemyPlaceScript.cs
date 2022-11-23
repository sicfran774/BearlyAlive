using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class EnemyPlaceScript
{
    public static void SaveEnemyPlacements(List<EnemyData> enemyOrder)
    {
        //Find AppData folder which will create a file on PC
        string path = Application.persistentDataPath + "/levelOne.json";

        //Convert array of EnemyData into a JSON file
        string json = JsonConvert.SerializeObject(enemyOrder, Formatting.Indented);
        System.IO.File.WriteAllText(path, json);
    }

    public static List<EnemyDataJson> LoadEnemyData(string path)
    {
        string data = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<List<EnemyDataJson>>(data);
    }
}
