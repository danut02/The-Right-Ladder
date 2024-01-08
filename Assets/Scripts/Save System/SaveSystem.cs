using System;
using System.IO;
using UnityEngine;
using static MovementController;
public static class SaveSystem
{
    private static readonly string path = Application.persistentDataPath + "/playerSave.json";

    public static void SaveData()
    {
        Data data = new Data
        {
            level=Levels.Instance.getLevel(),
            xPosition = movementInstance.player.transform.position.x,
            yPosition = movementInstance.player.transform.position.y,
            zPosition = movementInstance.player.transform.position.z
        };
        string json = JsonUtility.ToJson(data);
        Debug.Log(json);
        File.WriteAllText(path, json);
    }

    public static Data LoadData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data data = JsonUtility.FromJson<Data>(json);
            return data;
        }
        else
        {
            Debug.LogError("Save file not found at " + path);
            return null;
        }
    }
}