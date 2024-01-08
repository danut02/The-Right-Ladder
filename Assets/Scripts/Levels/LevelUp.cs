using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Levels.Instance.setLevel(Levels.Instance.getLevel()+1);
        SaveSystem.SaveData();
        string objectName = "Level"+Levels.Instance.getLevel();
        Debug.Log(objectName);
        GameObject objectToDestroy = GameObject.Find(objectName);
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
    }
}
