using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MovementController;
using TMPro;

public class TestLadder : MonoBehaviour, IInteractable
{
    public DialogueTrigger trigger;
    public bool Interact(Interactor interactor)
    {
        if (movementInstance.velocity.y < -15f)
        {
            Debug.Log("Homunculus was not able to catch onto the ladder because he was falling too fast");
            return false;
        }
        DialogueManager.instance.OpenDialogue(trigger.messages,trigger.actors);
        Debug.Log(movementInstance.velocity.y);
        movementInstance.velocity.y = 0f;
        movementInstance.isOnLadder = !movementInstance.isOnLadder;
        return true;
    }
}