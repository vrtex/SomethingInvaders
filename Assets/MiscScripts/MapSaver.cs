using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapSaver : MonoBehaviour
{
    public string filename;

    void Save()
    {
        EnemyController[] aliens = FindObjectsOfType<EnemyController>();

        string enemyData = "";
        foreach(EnemyController enemy in aliens)
        {
            enemyData += enemy.gameObject.transform.position.ToString() + " ";
            enemyData += enemy.type.ToString() + System.Environment.NewLine;
        }

        string path = "Assets/Maps/" + filename + ".txt";

        StreamWriter writer = new StreamWriter(path);
        writer.Write(enemyData);
        writer.Close();
        Debug.Log("saved to file: " + filename);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)) Save();
    }
}
