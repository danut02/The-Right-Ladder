using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovementController;

public class MissingStairs : MonoBehaviour
{
    public Collider myCollider;
    private bool isCollidingWithPlayer = false; // A flag to track continuous collision

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider has the "Player" tag
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("Am intrat pe fraier");
            isCollidingWithPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset the flag when the collision ends
        if (collision.collider.CompareTag("Player"))
        {
            //Debug.Log("Am iesit de pe fraier");
            isCollidingWithPlayer = false;
        }
    }

    private void Update()
    {
        if (movementInstance.isOnLadder)
        {
            // Check for continuous collision and key press
            if (isCollidingWithPlayer && Input.GetKey(KeyCode.Space))
            {
                //Debug.Log("Am sarit pe fraier");
                myCollider.enabled = false; // Deactivate the collider
                movementInstance.JumpOnLadder();
            }
            else
            {
                // Reactivate the collider when the player exits
                myCollider.enabled = true;
                //Debug.Log("Collider reactivated");
            }

            if (movementInstance.ctrlPressed)
            {
                myCollider.enabled = false;
                movementInstance.SlideOnLadder();
            }
        }
        else
        {
            myCollider.enabled = false;
        }
        
    }
}