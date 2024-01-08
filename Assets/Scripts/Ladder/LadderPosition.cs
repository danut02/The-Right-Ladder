using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovementController;
public class LadderPosition : MonoBehaviour
{
    private int pos = 0;
    //0 fata mijloc
    //1 dreapta fata
    //2 stanga spate
    //3 mijloc spate
    //4 dreapta spate
    //5 stanga fata
    Vector3 newPosition;
    Quaternion newRotation;
    public Stamina staminaScript;

    public GameObject ladder;
    
    public static LadderPosition ladderPosition;
    private void Awake() => ladderPosition = this;
    
    private void Start()
    {
        Vector3 newPosition = movementInstance.player.transform.position;
    }

    void Update()
    {
        if (!movementInstance.isOnLadder)
            return;
        InputForPos();
        PositionHandler();
    }

    void InputForPos()
    {
        if (!staminaScript.isResting)
        {
            if (Input.GetKeyDown(KeyCode.A))
                if (pos == 0)
                    pos = 5;
                else pos--;
            if (Input.GetKeyDown(KeyCode.D))
                if (pos == 5)
                    pos = 0;
                else pos++;
        }
    }
    
    public void PositionHandler()
    {
        if (staminaScript.isResting)
        {
            return;
        }
        var position = ladder.transform.position;
        //204 166
        switch (pos)
        {
            
            case 0: //0 fata mijloc 
                ChangePosRot(position.x + 0.55f,position.z + 32.75f,180f); //204.5 199
                break;
            case 1: //1 dreapta fata 
                ChangePosRot(position.x,position.z + 32.75f,140f); //203.5 199
                break;
            case 2: //2 stanga spate 
                ChangePosRot(position.x,position.z + 32.5f,50f);  //203.5 198
                break;
            case 3: //3 mijloc spate 
                ChangePosRot(position.x + 0.65f,position.z + 32.45f,0f);   //204.5 198
                break;
            case 4: //4 dreapta spate
                ChangePosRot(position.x + 1.25f,position.z + 32.45f,-50f); //205.5 198
                break;
            case 5: //5 stanga fata 
                ChangePosRot(position.x + 1.2f,position.z + 32.75f,-140f); //205.5 199
                break;
            default: Debug.Log("Pozitie invalida");
                break;
        }
    }
    
    void ChangePosRot(float posX, float posZ, float rotY)
    {
        newPosition = movementInstance.player.transform.position; // Get the current position
        newPosition.x = posX; // Modify the x component of position
        newPosition.z = posZ; // Modify the z component of position
        movementInstance.player.transform.position = newPosition; // Set the position back
        newRotation = movementInstance.player.transform.rotation; // Get the current rotation
        movementInstance.player.transform.rotation = Quaternion.Euler(0,rotY,0); // Set the rotation back
    }
}
