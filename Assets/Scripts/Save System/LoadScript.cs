using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using static MovementController;
public class LoadScript
{
    public Vector3 direction;
    public void Load()
    {
        Data data = SaveSystem.LoadData();
        if (data != null)
        {
            direction = new Vector3(data.xPosition, data.yPosition, data.zPosition);
   //         movementInstance.LoadMovement(direction);
            Levels.Instance.setLevel(data.level);
        }
    }
}
