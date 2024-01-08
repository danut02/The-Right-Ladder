using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static MovementController;
using TMPro;

public class Ladder : MonoBehaviour, IInteractable
{
    public bool Interact(Interactor interactor)
    {
        // Check if the player is facing the ladder
        RaycastHit hit;
        if (Physics.Raycast(interactor.transform.position, interactor.transform.forward, out hit, 1f))
        {
            if (hit.collider.gameObject.CompareTag("Ladder"))
            {
                // If the player is falling too fast, they cannot interact with the ladder
                if (movementInstance.velocity.y < -15f)
                {
                    Debug.Log("Homunculus was not able to catch onto the ladder because he was falling too fast");
                    return false;
                }

                // Set the player's velocity to 0 and toggle the isOnLadder flag
                movementInstance.velocity.y = 0f;
                movementInstance.isOnLadder = !movementInstance.isOnLadder;

                // Display a message in the monologue
                Monologue.instance.DisplayText("I interacted with the ladder!", 2);

                return true;
            }
        }

        // If the player is not facing the ladder, return false
        return false;
    }
}