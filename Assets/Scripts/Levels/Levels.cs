using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Levels : MonoBehaviour
{
    public static Levels Instance = new Levels();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private int level = 0;
    
    
    public void setLevel(int value)
    {
        level = value;
    }
    public int getLevel()
    {
        return level;
    }
    
    
}
